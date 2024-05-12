using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;
using UnityEngine.UI;


public class BattleManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject go;

    [SerializeField]
    private GameObject aimPoint;

    [SerializeField]
    private GameObject missile;

    [SerializeField]
    private GameObject missile2;

    [SerializeField] 
    private GameObject explose;

    [SerializeField]
    private GameObject splash;

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

    private GlobalDataSingleton instance; 

    void Start()
    {
        StartCoroutine(Init());       
    }

    IEnumerator Init() // 전투를 시작하기 전에 오브젝트들을 셋팅한다.
    {
        instance = GlobalDataSingleton.GetSingletonInstance();

        enemyTile = instance.EnemyTile;
        myTile = instance.MyTile;

        missTile = Resources.Load<TileBase>("Tiles/tile_green");
        hitTile = Resources.Load<TileBase>("Tiles/tile_red");

        aimPoint = ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.aimpoint); 
        missile = ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.missile);
        missile2 = ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.missile2); 

        explose = ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.explose);
        splash = ObjectPool.GetPoolObject((int)ObjectPool.ObjectType.splash);

        if (PhotonNetwork.IsMasterClient)
        {
            instance.isMyTurn = true;
            myTurnUI.gameObject.SetActive(true);
        }

        else
        {
            enemyTurnUI.gameObject.SetActive(true);
        }

        instance.isReady = true;

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
                StartCoroutine(AttackCoroutine()); // 코루틴 시작
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        // AttackAndExplode RPC 호출
        photonView.RPC("Attack", RpcTarget.All, enemyTile.GetCellCenterWorld(targetPosition), PhotonNetwork.IsMasterClient);

        // Attack이 완료될 때까지 대기
        while (instance.isAttacking)
        {
            yield return null;
        }

        if (instance.isHit == true) // 공격이 맞으면 explose 실행
        {
            photonView.RPC("Explose", RpcTarget.All, enemyTile.GetCellCenterWorld(targetPosition));
        }

        else // 빗나가면 splash 실행
        {
            photonView.RPC("Splash", RpcTarget.All, enemyTile.GetCellCenterWorld(targetPosition));
        }

        while (instance.isMyTurn) // 턴이 끝나기 전까지 대기 
        {
            yield return null;
        }

        if(instance.isDestory) // 파괴된 함선이 있는지 체크한다.
        {
            SetDestroiedShip(); // 상대의 함선이 파괴되면 그 위치에 함선을 생성한다.

            yield return SetDestroiedShip();
        }

        if (instance.isFinish) // 게임이 끝났다는 것이 확인이 되면 게임을 종료한다.
        {
            FinishGame();
        }

        instance.isHit = false;

        photonView.RPC("TurnEnd", RpcTarget.Others); // 상대방에게 턴을 넘긴다.
        ChangeTurnUI();
    }

   public void SetObject(GameObject go, Vector3 spawnPosition)
    {
        go.transform.position = spawnPosition;
        go.transform.rotation = Quaternion.identity;
        go.SetActive(true);
    }

    [PunRPC]
    public void Attack(Vector3 spawnPosition, bool player1)
    {
        SetObject(aimPoint, spawnPosition); // 조준점을 셋팅한다.

        if (player1) // 방장이면 미사일을 아래에서 쏜다
        {
            spawnPosition.y -= 10f;
            SetObject(missile, spawnPosition);
        }

        else // 플레이어2면 위에서 미사일을 쏜다
        {
            spawnPosition.y += 10f;
            SetObject(missile2, spawnPosition);
        }
    }

    [PunRPC]
    public void Explose(Vector3 spawnPosition)
    {
        SetObject(explose, spawnPosition);

        if (instance.isMyTurn) // 공격을 실행한 자리에 빨간색 타일을 셋팅한다.
        {
            enemyTile.SetTile(enemyTile.WorldToCell(spawnPosition), hitTile);
        }

        else // 공격을 받은 플레이어는 자신의 타일맵에 타일을 셋팅한다. 
        {
            myTile.SetTile(myTile.WorldToCell(spawnPosition), hitTile);
        } 

        if(instance.isMyTurn) instance.isMyTurn = false;
    }

    [PunRPC]
    public void Splash(Vector3 spawnPosition)
    {
        SetObject(splash, spawnPosition);

        if (instance.isMyTurn) // 공격을 실행한 자리에 초록색 타일을 셋팅한다.
        {
            enemyTile.SetTile(enemyTile.WorldToCell(spawnPosition), missTile);
        }

        else // 공격을 받은 유저는 자신의 타일맵에 타일을 셋팅한다.
        {
            myTile.SetTile(myTile.WorldToCell(spawnPosition), missTile);
        }

        if (instance.isMyTurn) instance.isMyTurn = false;
    }

    [PunRPC]
    public void TurnEnd() // 상대에게 rpc로 턴이 끝났다는 것을 알려주는 메소드다.
    {
        if(instance.isMyTurn == false) instance.isMyTurn = true;

        ChangeTurnUI();
    }

    public void ChangeTurnUI() //턴이 변경될 때마다 턴을 표시해주는 ui를 변경하는 메소드다.
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

    

    IEnumerator SetDestroiedShip() // 상대의 함선을 격침 시켰으면 그 위치에 함선을 표시하는 메소드다.
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
        if(instance.Hp > 0) // 체력이 0보다 크면 이겼다는 메시지를
        {
            instance.SetPopUp("You Win");
        }

        else // 0 보다 작으면 패배했다는 메시지를 띄운다.
        {
            instance.SetPopUp("You Lose");
        }

        yield return new WaitForSeconds(3);

        PhotonNetwork.LeaveRoom();
         // 3초 후 게임을 종료한다.
    }
}
