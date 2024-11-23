using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCount : MonoBehaviour
{
    void Update(){
        PlayerMove[] players = FindObjectsOfType<PlayerMove>();
        int totalMoney = 0;
        foreach(PlayerMove player in players){
            totalMoney += player.Money;
        }
        GetComponent<TextMeshProUGUI>().text = $"{totalMoney}";
    }
}
