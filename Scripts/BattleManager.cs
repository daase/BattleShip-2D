using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject go;

    [SerializeField]
    private Tilemap enemyTile;

    [SerializeField]
    private Tilemap myTile;

    private TileBase target;
    private TileBase hitTile;
    private TileBase missTile;

    private Vector2 mousePos;
    private Vector3Int targetPosition;

    [SerializeField]
    private Image myTurnUI;

    [SerializeField]
    private Image enemyTurnUI;

    private AudioSource audioSource;
    private GlobalDataSingleton instance; 

    void Start()
    {
        StartCoroutine(Init());       
    }

    IEnumerator Init()
    {
        instance = GlobalDataSingleton.GetSingletonInstance();

        enemyTile = instance.EnemyTile;
        myTile = instance.MyTile;

        missTile = Resources.Load<TileBase>("Tiles/tile_green");
        hitTile = Resources.Load<TileBase>("Tiles/tile_red");

        ObjectPool.InitPool();

        if (PhotonNetwork.IsMasterClient)
        {
            instance.isMyTurn = true;
            myTurnUI.gameObject.SetActive(true);
        }

        else
        {
            enemyTurnUI.gameObject.SetActive(true);
        }
        yield return null;
    }

    private void Update()
    {
        SetAttackPoint();
    }

    public void SetAttackPoint()
    {
        if (Input.GetMouseButtonDown(0) && instance.isReady && instance.isAttacking == false && instance.isMyTurn)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = enemyTile.WorldToCell(mousePos);
            target = enemyTile.GetTile(targetPosition);

            if (target != null && target.name == "tile" && instance.isAttacking == false)
            {
                instance.isAttacking = true;
                StartCoroutine(AttackCoroutine()); // �ڷ�ƾ ����
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        // AttackAndExplode RPC ȣ��
        photonView.RPC("Attack", RpcTarget.All, enemyTile.GetCellCenterWorld(targetPosition), PhotonNetwork.IsMasterClient);

        // Attack�� �Ϸ�� ������ ���
        while (instance.isAttacking)
        {
            yield return null;
        }

        if (instance.isHit == true) // ������ ������ explose ����
        {
            photonView.RPC("Explose", RpcTarget.All, enemyTile.GetCellCenterWorld(targetPosition));
        }

        else // �������� splash ����
        {
            photonView.RPC("Splash", RpcTarget.All, enemyTile.GetCellCenterWorld(targetPosition));
        }

        while (instance.isMyTurn) // ���� ������ ������ ��� 
        {
            yield return null;
        }

        if(instance.isDestory) // �ı��� �Լ��� �ִ��� üũ�Ѵ�.
        {
            SetDestroiedShip(); // ����� �Լ��� �ı��Ǹ� �� ��ġ�� �Լ��� �����Ѵ�.

            yield return SetDestroiedShip();
        }

        if (instance.isFinish) // ������ �����ٴ� ���� Ȯ���� �Ǹ� ������ �����Ѵ�.
        {
            FinishGame();
        }

        instance.isHit = false;

        photonView.RPC("TurnEnd", RpcTarget.Others); // ���濡�� ���� �ѱ��.
        ChangeTurnUI();
    }

   

    [PunRPC]
    public void Attack(Vector3 spawnPosition, bool player1)
    {
        go = ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.aimpoint); // �������� �����Ѵ�.
        go.transform.position = spawnPosition;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);
        PlaySound();

        if (player1) // �����̸� �̻����� �Ʒ����� ���
        {
            go = ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.missile);
            spawnPosition.y -= 10f;
        }

        else // �÷��̾�2�� ������ �̻����� ���
        {
            go = ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.missile2);
            spawnPosition.y += 10f;
        }
        
        go.transform.position = spawnPosition;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);
        PlaySound();
    }

    [PunRPC]
    public void Explose(Vector3 spawnPosition)
    {
        go = ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.explose); // ���� ������Ʈ�� Ȱ��ȭ�Ѵ�.
        go.transform.position = spawnPosition;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);
        PlaySound();

        if (instance.isMyTurn) // ������ ������ �ڸ��� ������ Ÿ���� �����Ѵ�.
        {
            enemyTile.SetTile(enemyTile.WorldToCell(spawnPosition), hitTile);
        }

        else // ������ ���� �÷��̾�� �ڽ��� Ÿ�ϸʿ� Ÿ���� �����Ѵ�. 
        {
            myTile.SetTile(myTile.WorldToCell(spawnPosition), hitTile);
        } 

        if(instance.isMyTurn) instance.isMyTurn = false;
    }

    [PunRPC]
    public void Splash(Vector3 spawnPosition)
    {
        go = ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.splash); // splash ������Ʈ�� Ȱ��ȭ�Ѵ�.
        go.transform.position = spawnPosition;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);
        PlaySound();

        if (instance.isMyTurn) // ������ ������ �ڸ��� �ʷϻ� Ÿ���� �����Ѵ�.
        {
            enemyTile.SetTile(enemyTile.WorldToCell(spawnPosition), missTile);
        }

        else // ������ ���� ������ �ڽ��� Ÿ�ϸʿ� Ÿ���� �����Ѵ�.
        {
            myTile.SetTile(myTile.WorldToCell(spawnPosition), missTile);
        }

        if (instance.isMyTurn) instance.isMyTurn = false;
    }

    [PunRPC]
    public void TurnEnd() // ��뿡�� rpc�� ���� �����ٴ� ���� �˷��ִ� �޼ҵ��.
    {
        if(instance.isMyTurn == false) instance.isMyTurn = true;

        ChangeTurnUI();
    }

    public void ChangeTurnUI() //���� ����� ������ ���� ǥ�����ִ� ui�� �����ϴ� �޼ҵ��.
    {
        if (myTurnUI.gameObject.activeSelf)
        {
            myTurnUI.gameObject.SetActive(false);
            enemyTurnUI.gameObject.SetActive(true);
        }

        else
        {
            enemyTurnUI.gameObject.SetActive(false);
            myTurnUI.gameObject.SetActive(true);
        }
    }

    public void PlaySound()
    {
        audioSource = go.GetComponent<AudioSource>();
        audioSource.Play();
    }

    IEnumerator SetDestroiedShip() // ����� �Լ��� ��ħ �������� �� ��ġ�� �Լ��� ǥ���ϴ� �޼ҵ��.
    {
        go = Resources.Load<GameObject>($"Prefabs/{instance.EnemyShipName}");
        Instantiate(go, instance.EnemyShipPosition, Quaternion.Euler(0, 0, instance.EnemyShipAngle));

        instance.isDestory = false;

        while (instance.isDestory)
        {
            yield return null;
        }
    }

    [PunRPC]
    public void FinishGame()
    {
        StartCoroutine(SetFinishPopUp());
    }

    IEnumerator SetFinishPopUp()
    {
        if(instance.Hp > 0) // ü���� 0���� ũ�� �̰�ٴ� �޽�����
        {
            instance.SetPopUp("You Win");
        }

        else // 0 ���� ������ �й��ߴٴ� �޽����� ����.
        {
            instance.SetPopUp("You Lose");
        }

        yield return new WaitForSeconds(3);

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LobbyScene"); // 3�� �� ������ �����Ѵ�.
    }
}
