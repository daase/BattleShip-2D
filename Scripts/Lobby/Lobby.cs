using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject popUp; // 팝업창
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
        
        if(AuthManager.firebaseAuth.CurrentUser != null) // 로그인 상태 확인
        {
            PhotonNetwork.Disconnect();
            AuthManager.firebaseAuth.SignOut(); // 로그아웃           
            SceneManager.LoadScene("MenuScene"); // 시작 화면으로 이동          
        }       
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnCreateRoom()
    {
        PhotonNetwork.CreateRoom(roomNameField.text, new Photon.Realtime.RoomOptions { MaxPlayers =2});
    }// 최대인원 2명으로 방 생성

    public void SetPopUp(string message) // 팝업창을 띄우는 함수
    {
        popUpText.text = message;
        popUp.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetPopUp("게임 참가에 실패했습니다.");
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }
}
