using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isSpawnTower { set; get; }

    private void Awake()
    {
        isSpawnTower = false;
    }
}
