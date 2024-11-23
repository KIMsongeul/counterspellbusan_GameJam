using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawn : MonoBehaviour
{
    public GameObject towerPrefab;

    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.isSpawnTower == true)
        {
            return;
        }

        tile.isSpawnTower = true;
        
        Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
    }
}
