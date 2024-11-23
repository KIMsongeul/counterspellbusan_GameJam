using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    public GameObject gameOverUI;
    private bool isTagSet = false;

    public bool isGameInProgress = false;
    public PhotonView pv;

    void Awake(){
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        //pv = GetComponent<PhotonView>();
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

        StartCoroutine(StartGameWithDelay());
    }

    IEnumerator StartGameWithDelay()
    {
        yield return new WaitForSeconds(1f);
        SetPlayerTags();
        isTagSet = true;
        isGameInProgress = true;
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

    [PunRPC]
    public void GameOver(){
        PhotonNetwork.LeaveRoom();
    }

    override public void OnLeftRoom(){
        gameOverUI.SetActive(true);
    }

    public void ReturnMain(){
        PhotonNetwork.LoadLevel("Main");
    }
}
