using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviourPunCallbacks
{       
    private GlobalDataSingleton instance;

    [SerializeField]
    private Button playBtn;

    [SerializeField]
    private GameObject popUp;

    [SerializeField]
    private Text popUpText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        instance = GlobalDataSingleton.GetSingletonInstance();
        instance.Ships = GameObject.FindGameObjectsWithTag("Ship");

        playBtn.interactable = false; // play��ư ��Ȱ��ȭ

        instance.popUp = popUp;
        instance.popUpText = popUpText;

        ObjectPool.InitPool(); // ������Ʈ Ǯ�� ������Ʈ���� �̸� �־�д�.

        foreach (var ship in instance.Ships)
        {
            ship.SetActive(false); // ���ӽ��� �� ��Ȱ��ȭ           
        }
        CheckPlayerCount();

        yield return null;
    }

    // �÷��̾� ���� Ȯ���ϰ� �׿� ���� ���� ������Ʈ Ȱ��ȭ/��Ȱ��ȭ
    void CheckPlayerCount()
    {
      
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

       
        if (playerCount == 2) // �÷��̾� ���� 2�� �̸��̸� ���� ������Ʈ ��Ȱ��ȭ
        {

            if (PhotonNetwork.IsMasterClient) // �����̸� �÷��̾� 1�� ����
            {
                foreach (var ship in instance.Ships)
                {
                    ship.SetActive(true);
                }

                instance.MyTile = GameObject.Find("MyTile").GetComponent<Tilemap>();
                instance.EnemyTile = GameObject.Find("MyTile2").GetComponent<Tilemap>(); // �Ʒ��� Ÿ�ϸ��� �� Ÿ�ϸ�
            }

            else // ������ �ƴϸ� �÷��̾� 2�� ����
            {
               foreach(var ship in instance.Ships)
               {
                    ship.SetActive(true);
                    ship.transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y + 4.78f, 0);
                }

               instance.MyTile = GameObject.Find("MyTile2").GetComponent<Tilemap>();
               instance.EnemyTile = GameObject.Find("MyTile").GetComponent<Tilemap>(); // ���� Ÿ�ϸ��� �� Ÿ�ϸ�
            }

            instance.Hp = 14;
            playBtn.interactable = true; // play��ư Ȱ��ȭ
        }
    }
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) // ���� �ο� ���� �ٲ�� �ٽ� �ο��� üũ�Ѵ�.
    {       
        CheckPlayerCount();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        StartCoroutine(NoticePlayerOut());
    }

    IEnumerator NoticePlayerOut()
    {
        instance.SetPopUp("��밡 ������ �����߽��ϴ�.");

        yield return new WaitForSeconds(3f);

        PhotonNetwork.LeaveRoom(); //��밡 ������ 3�� �ڿ� ������ �����Ѵ�.
    }

    public override void OnLeftRoom() // ���� ���� �� lobby���� �ҷ��´�.
    {
        SceneManager.LoadScene("LobbyScene");
    }

}
