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
   
    public int playersReady = 0; // �غ�� �÷��̾��� �� 

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
        playBtn.interactable = false; // play ��ư�� ������ �ٽ� ���� �� ������ ��Ȱ��ȭ �Ѵ�.

        photonView.RPC("OnPlayerReady", RpcTarget.Others); // �׸��� ���濡�� play��ư�� �����ٴ� ���� rpc�� �˸���.

        if (playersReady == 2)
        {
           OnPlayerReady();
        }
        
    }

    [PunRPC]
    private void OnPlayerReady()
    {
        if (playersReady < 2) playersReady++; // ��ư�� �������� �ش� ���� �ø���.

        if ((playersReady == 2)) // 2�� ��� ��ư�� �����ٸ� battlemanager�� Ȱ��ȭ�ϰ� ������ �����Ѵ�. 
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
                Destroy(ships[i].GetComponent<DragDrop>()); // �巡�� ��� ����� �����Ѵ�
            }

            if (ships[i].GetComponent<ShipRotateManager>() != null)
            {
                Destroy(ships[i].GetComponent<ShipRotateManager>()); // ȸ�� ����� �����Ѵ�
            }
        }
    }
   
}
