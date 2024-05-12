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

        playBtn.interactable = false; // play버튼 비활성화

        instance.popUp = popUp;
        instance.popUpText = popUpText;

        ObjectPool.InitPool(); // 오브젝트 풀에 오브젝트들을 미리 넣어둔다.

        foreach (var ship in instance.Ships)
        {
            ship.SetActive(false); // 게임시작 전 비활성화           
        }
        CheckPlayerCount();

        yield return null;
    }

    // 플레이어 수를 확인하고 그에 따라 게임 오브젝트 활성화/비활성화
    void CheckPlayerCount()
    {
      
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

       
        if (playerCount == 2) // 플레이어 수가 2명 미만이면 게임 오브젝트 비활성화
        {

            if (PhotonNetwork.IsMasterClient) // 방장이면 플레이어 1로 설정
            {
                foreach (var ship in instance.Ships)
                {
                    ship.SetActive(true);
                }

                instance.MyTile = GameObject.Find("MyTile").GetComponent<Tilemap>();
                instance.EnemyTile = GameObject.Find("MyTile2").GetComponent<Tilemap>(); // 아래쪽 타일맵이 내 타일맵
            }

            else // 방장이 아니면 플레이어 2로 설정
            {
               foreach(var ship in instance.Ships)
               {
                    ship.SetActive(true);
                    ship.transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y + 4.78f, 0);
                }

               instance.MyTile = GameObject.Find("MyTile2").GetComponent<Tilemap>();
               instance.EnemyTile = GameObject.Find("MyTile").GetComponent<Tilemap>(); // 위쪽 타일맵이 내 타일맵
            }

            instance.Hp = 14;
            playBtn.interactable = true; // play버튼 활성화
        }
    }
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) // 방의 인원 수가 바뀌니 다시 인원을 체크한다.
    {       
        CheckPlayerCount();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        StartCoroutine(NoticePlayerOut());
    }

    IEnumerator NoticePlayerOut()
    {
        instance.SetPopUp("상대가 게임을 종료했습니다.");

        yield return new WaitForSeconds(3f);

        PhotonNetwork.LeaveRoom(); //상대가 없으니 3초 뒤에 게임을 종료한다.
    }

    public override void OnLeftRoom() // 방을 나갈 때 lobby씬을 불러온다.
    {
        SceneManager.LoadScene("LobbyScene");
    }

}
