using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSpawn : MonoBehaviour
{
    public GameObject towerPrefab;

    public GameObject tower1;
    public GameObject tower2;

    GameManager gameManager;

    PhotonView pv;

    PlayerMove playerMove;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        pv = GetComponent<PhotonView>();
        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer(){
        yield return new WaitForSeconds(0.5f);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players){
            if(player.GetComponent<PhotonView>().IsMine){
                playerMove = player.GetComponent<PlayerMove>();
                break;
            }
        }
    }

    public void SpawnTower(Transform tileTransform)
    {
        if(!gameManager.isGameInProgress){
            return; // 게임이 진행중이 아니면 타워 생성 안함    
        }
        if(towerPrefab == tower1){
            if(playerMove.Money < 500){
                return;
            }
            else{
                playerMove.Money -= 500;
            }
        }
        else if(towerPrefab == tower2){
            if(playerMove.Money < 1000){
                return;
            }
            else{
                playerMove.Money -= 1000;
            }
        }
        Tile tile = tileTransform.GetComponent<Tile>();
        if (tile.isSpawnTower == true)
        {
            return;
        }
        
        GameObject tower = PhotonNetwork.Instantiate($"Prefabs/{towerPrefab.name}", tileTransform.position, Quaternion.identity);
        PhotonView towerPhotonView = tower.GetComponent<PhotonView>();
        
        if (towerPhotonView != null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                towerPhotonView.RPC("SetColor", RpcTarget.All, 0f, 0f, 1f);
                GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
                if (enemy != null)
                {
                    tower.GetComponent<TowerAttack>().AssignTarget(enemy.transform);
                }
            }
            else 
            {
                towerPhotonView.RPC("SetColor", RpcTarget.All, 1f, 0f, 0f);
                GameObject enemy = GameObject.FindGameObjectWithTag("Player");
                if (enemy != null)
                {
                    tower.GetComponent<TowerAttack>().AssignTarget(enemy.transform);
                }
            }
        }
        
        pv.RPC("SpawnTowerRPC", RpcTarget.All, tileTransform.GetInstanceID(), tower.GetInstanceID());
    }

    [PunRPC]
    void SpawnTowerRPC(int tileInstanceId, int towerInstanceId)
    {
        print("SpawnTowerRPC");
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach(GameObject tileObj in tiles)
        {
            if(tileObj.GetInstanceID() == tileInstanceId)
            {
                tileObj.GetComponent<Tile>().isSpawnTower = true;
                break;
            }       
        }
    }
}
