using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MainManager : MonoBehaviourPunCallbacks
{
    public GameObject loadingUI;
    public GameObject CodeInputPannel;

    void Start()
    {
        print("Connecting to Lobby");
        loadingUI.SetActive(true); // 로딩 UI 활성화
        PhotonNetwork.ConnectUsingSettings(); // 먼저 마스터 서버에 연결
    }

    public override void OnConnectedToMaster()
    {
        print("Connected to Master");
        PhotonNetwork.JoinLobby(); // 마스터 서버 연결 후 로비 접속 시도
    }

    public override void OnJoinedLobby()
    {
        print("Entered Lobby");
        loadingUI.SetActive(false); // 로비 접속 완료시 로딩 UI 비활성화
    }

    public void StartGame_Btn(){
        CodeInputPannel.SetActive(true);
    }   
}
