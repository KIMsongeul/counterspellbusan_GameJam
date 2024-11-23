using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    private bool isTagSet = false;

    void Start()
    {
        GameObject player;
        if(PhotonNetwork.IsMasterClient)
        {
            player = PhotonNetwork.Instantiate($"Prefabs/{playerPrefab.name}", spawnPoints[0].position, Quaternion.identity);
            PhotonView photonView = player.GetComponent<PhotonView>();
            if(photonView != null)
            {
                photonView.RPC("SetColor", RpcTarget.All, 0f, 0f, 1f);
            }
        }
        else
        {
            player = PhotonNetwork.Instantiate($"Prefabs/{playerPrefab.name}", spawnPoints[1].position, Quaternion.identity);
            PhotonView photonView = player.GetComponent<PhotonView>();
            if(photonView != null)
            {
                photonView.RPC("SetColor", RpcTarget.All, 1f, 0f, 0f);
            }
        }

        // 씬 로드 후 약간의 딜레이를 두고 태그 설정
        StartCoroutine(DelayedTagSetup());
    }

    IEnumerator DelayedTagSetup()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SetPlayerTags();
        isTagSet = true;
    }

    void SetPlayerTags()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            PhotonView pv = p.GetComponent<PhotonView>();
            if (pv != null)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    if (!pv.IsMine)
                    {
                        p.tag = "Red";
                    }
                    else
                    {
                        p.tag = "Blue";
                    }
                }
                else
                {
                    if (pv.IsMine)
                    {
                        p.tag = "Red";
                    }
                    else
                    {
                        p.tag = "Blue";
                    }
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (isTagSet)
        {
            SetPlayerTags();
        }
    }
}
