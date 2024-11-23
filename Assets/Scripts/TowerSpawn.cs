using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TowerSpawn : MonoBehaviour
{
    public GameObject towerPrefab;

    PhotonView pv;

    void Awake(){
        pv = GetComponent<PhotonView>();
    }

    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.isSpawnTower == true)
        {
            return;
        }

        tile.isSpawnTower = true;
        pv.RPC("SpawnTowerRPC", RpcTarget.All, tileTransform.position);
    }

    [PunRPC]
    void SpawnTowerRPC(Vector3 position)
    {
        print("SpawnTowerRPC"); 
        Instantiate(towerPrefab, position, Quaternion.identity);
    }
}
