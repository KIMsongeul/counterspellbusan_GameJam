using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject turretPrefab;
    TowerSpawn towerSpawn;

    void Awake(){
        towerSpawn = FindObjectOfType<TowerSpawn>();
    }

    void Update(){
        if(towerSpawn.towerPrefab == turretPrefab){
            GetComponent<RectTransform>().sizeDelta = new Vector2(120, 120);
        }
        else{
            GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        }
    }

    public void OnClick(){
        print("TurretButton OnClick");
        towerSpawn.towerPrefab = turretPrefab;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick();
    }
}
