using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TowerSpawn : MonoBehaviour
{
    public GameObject towerPrefab;
    GameManager gameManager;

    PhotonView pv;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        pv = GetComponent<PhotonView>();
    }

    public void SpawnTower(Transform tileTransform)
    {
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
