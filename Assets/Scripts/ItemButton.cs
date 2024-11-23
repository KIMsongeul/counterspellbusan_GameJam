using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour, IPointerDownHandler
{
    private bool isItemActive = false;
    private GameObject selectedTower = null;
    
    public void OnClick(){
        print("ItemButton OnClick");
    }

    public void OnPointerDown(PointerEventData eventData){
        print("ItemButton OnPointerDown");
        
        if (isItemActive)
        {
            return;
        }
        isItemActive = true;

        StartCoroutine(SelectTower());
    }

    private IEnumerator SelectTower()
    {
        while (selectedTower == null)
        {   
            if (Input.GetMouseButtonUp(0))
            {   
                Debug.Log("ray 쏘기");
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                if (hit.collider.CompareTag("Tower"))
                {
                    Debug.Log("타워선택 ");
                    selectedTower = hit.collider.gameObject;
                    ApplyItemToTower(selectedTower);
                    yield break;
                }
            }
            yield return null;
        }
    }
    
    private void ApplyItemToTower(GameObject tower)
    {
        Debug.Log("아이템적용");
        TowerAttack towerAttack = tower.GetComponent<TowerAttack>();
        if (towerAttack != null)
        {
            towerAttack.pv.RPC("SetBulletSizeMultiplier", RpcTarget.All, 3f, 2f);
            // towerAttack.SetBulletSizeMultiplier(3f, 2f);
        }

        isItemActive = false;
        selectedTower = null;
    }
}
