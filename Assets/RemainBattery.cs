using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RemainBattery : MonoBehaviour
{
    public String tagName;
    public GameObject[] batteryImages;

    PlayerMove playerMove;
    PhotonView pv;

    void Awake(){
        pv = GetComponent<PhotonView>();
        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer(){
        yield return new WaitForSeconds(3f);
        playerMove = GameObject.FindGameObjectWithTag(tagName).GetComponent<PlayerMove>();
    }

    void Update(){
        if(playerMove == null){
            print("playerMove is null");
            return;
        }
        UpdateBatteryImages(playerMove.HP);
    }

    [PunRPC]
    public void UpdateBatteryImages(int hp){
        print(hp);
        // 배터리 이미지 배열을 순회하면서 플레이어의 현재 체력에 따라 배터리 이미지를 활성화/비활성화
        switch(playerMove.HP){
            case 3:
                batteryImages[0].SetActive(true);
                batteryImages[1].SetActive(true);
                batteryImages[2].SetActive(true);
                break;
            case 2:
                batteryImages[0].SetActive(true);
                batteryImages[1].SetActive(true);
                batteryImages[2].SetActive(false);
                break;
            case 1:
                batteryImages[0].SetActive(true);
                batteryImages[1].SetActive(false);
                batteryImages[2].SetActive(false);
                break;
            case 0:
                batteryImages[0].SetActive(false);
                batteryImages[1].SetActive(false);
                batteryImages[2].SetActive(false);
                break;
        }
    }
}
