using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class InputPannel : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomCodeInputField;

    public void EnterRoom_Btn()
    {
        PhotonNetwork.JoinRoom(roomCodeInputField.text);
    }

    public void CreateRoom_Btn()
    {
        string roomCode = Random.Range(100000, 1000000).ToString();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomCode, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 성공");
        PhotonNetwork.LoadLevel("Room_ReadyGame"); // ReadyRoom 씬으로 이동
    }
}
