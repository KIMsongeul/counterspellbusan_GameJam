using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        // 포톤 서버 연결
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 연결되었습니다.");
        // 로비 입장
        PhotonNetwork.JoinLobby();
    }   
    // 룸 입장 성공시 호출
    public override void OnJoinedRoom()
    {
        Debug.Log("룸에 입장하였습니다.");
    }
}
