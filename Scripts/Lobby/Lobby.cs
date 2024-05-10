using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject popUp; // �˾�â
    [SerializeField] private Text popUpText;

    [SerializeField] private InputField roomNameField;

    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject waitingPanel;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }   
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        waitingPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public  void LogOut()
    {
        
        if(AuthManager.firebaseAuth.CurrentUser != null) // �α��� ���� Ȯ��
        {
            PhotonNetwork.Disconnect();
            AuthManager.firebaseAuth.SignOut(); // �α׾ƿ�           
            SceneManager.LoadScene("MenuScene"); // ���� ȭ������ �̵�          
        }       
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnCreateRoom()
    {
        PhotonNetwork.CreateRoom(roomNameField.text, new Photon.Realtime.RoomOptions { MaxPlayers =2});
    }// �ִ��ο� 2������ �� ����

    public void SetPopUp(string message) // �˾�â�� ���� �Լ�
    {
        popUpText.text = message;
        popUp.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetPopUp("���� ������ �����߽��ϴ�.");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}
