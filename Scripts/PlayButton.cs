using Photon.Pun;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayButton : MonoBehaviourPunCallbacks
{
    private Button playBtn;
    private Tilemap myTile;
    private Tilemap collisionTile;
    
    private GameObject ship;
    private GameObject[] ships;

    public GameObject battleManager;
   
    private GlobalDataSingleton instance = GlobalDataSingleton.GetSingletonInstance();
   
    public int playersReady = 0; // 준비된 플레이어의 수 

    public void OnPlayButtonClicked()
    {
        ships = GameObject.FindGameObjectsWithTag("Ship");
        
        collisionTile = GameObject.Find("TileCollision").GetComponent<Tilemap>();
        collisionTile.GetComponent<TilemapCollider2D>().enabled = false; 
       
        instance = GlobalDataSingleton.GetSingletonInstance();

        int length = ships.Length;

        RemoveShipsComponent(length); 

        playersReady++;
        playBtn = GameObject.Find("Canvas/PlayButton").GetComponent<Button>();
        playBtn.interactable = false; // play 버튼을 누르면 다시 누를 수 없도록 비활성화 한다.

        photonView.RPC("OnPlayerReady", RpcTarget.Others); // 그리고 상대방에게 play버튼을 눌렀다는 것을 rpc로 알린다.

        if (playersReady == 2)
        {
           OnPlayerReady();
        }
        
    }

    [PunRPC]
    private void OnPlayerReady()
    {
        if (playersReady < 2) playersReady++; // 버튼을 눌렀으니 해당 값을 올린다.

        if ((playersReady == 2)) // 2명 모두 버튼을 눌렀다면 battlemanager를 활성화하고 게임을 진행한다. 
        {
            playBtn = GameObject.Find("Canvas/PlayButton").GetComponent<Button>();
            playBtn.interactable = false;
            battleManager.SetActive(true);
        }
        
    }   

    public void RemoveShipsComponent(int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (ships[i].GetComponent<DragDrop>() != null)
            {
                Destroy(ships[i].GetComponent<DragDrop>()); // 드래그 드롭 기능을 제거한다
            }

            if (ships[i].GetComponent<ShipRotateManager>() != null)
            {
                Destroy(ships[i].GetComponent<ShipRotateManager>()); // 회전 기능을 제거한다
            }
        }
    }
   
}
