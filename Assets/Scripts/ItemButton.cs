using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour, IPointerDownHandler
{
    public void OnClick(){
        print("ItemButton OnClick");
    }

    public void OnPointerDown(PointerEventData eventData){
        print("ItemButton OnPointerDown");
        
    }
}
