using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomList : MonoBehaviourPunCallbacks
{
    [Header("UI")] public Transform roomListParent;
    public GameObject roomListItem;
    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(cachedRoomList.Count <= 0) // 캐시된 방 목록이 비어 있으면 
        {
            cachedRoomList = roomList; // 업데이트된 방을 추가한다.
        }

        else
        {

            foreach(var room in roomList)
            {

                for(int i=0; i<cachedRoomList.Count; i++)
                {

                    if (cachedRoomList[i].Name == room.Name)
                    {

                        List<RoomInfo> newList = cachedRoomList;

                        if (room.RemovedFromList)
                        {
                            newList.Remove(newList[i]);
                        }

                        else
                        {
                            newList[i] = room;
                        }

                        cachedRoomList = newList;
                    }

                }

            }

        }

        UpdateRoomList();
    }

    public void UpdateRoomList() // roomList ui 갱신하는 메소드
    {
        foreach(Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach(var room in cachedRoomList)
        {
            GameObject roomItem =  Instantiate(roomListItem, roomListParent);

            roomItem.transform.GetChild(0).GetComponent<Text>().text = room.Name;
            roomItem.transform.GetChild(1).GetComponent<Text>().text = room.PlayerCount + "/2";

            roomItem.GetComponent<RoomItemButton>().roomName = room.Name;
        }
    }

    public static void JoinRoomByName(string roomName) // 방에 입장하는 메소드
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}
