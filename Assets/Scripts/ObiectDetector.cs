using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObiectDetector : MonoBehaviour
{
    public TowerSpawn towerSpawn;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        mainCamera = Camera.main;
        towerSpawn = FindObjectOfType<TowerSpawn>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray,out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    towerSpawn.SpawnTower(hit.transform);
                }
            }
        }
    }
}
