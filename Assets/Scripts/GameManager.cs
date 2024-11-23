using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // 플레이어 프리팹
    public Transform[] spawnPoints; // 플레이어 스폰 위치

    void Start(){
        // 플레이어 생성 및 색상 설정
        GameObject player;
        if(PhotonNetwork.IsMasterClient)
        {
            player = PhotonNetwork.Instantiate($"Prefabs/{playerPrefab.name}", spawnPoints[0].position, Quaternion.identity);
        }
        else
        {
            player = PhotonNetwork.Instantiate($"Prefabs/{playerPrefab.name}", spawnPoints[1].position, Quaternion.identity);
        }

        // 플레이어 색상 설정
        SpriteRenderer sr = player.GetComponentInChildren<SpriteRenderer>();
        if(player.GetComponent<PhotonView>().IsMine)
        {
            sr.color = Color.blue; // 자신은 파란색
        }
    }
}
