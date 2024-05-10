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
        if(cachedRoomList.Count <= 0) // ĳ�õ� �� ����� ��� ������ 
        {
            cachedRoomList = roomList; // ������Ʈ�� ���� �߰��Ѵ�.
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

    public void UpdateRoomList() // roomList ui �����ϴ� �޼ҵ�
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

    public static void JoinRoomByName(string roomName) // �濡 �����ϴ� �޼ҵ�
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}
