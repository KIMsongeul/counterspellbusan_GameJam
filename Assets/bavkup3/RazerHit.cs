using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RazerHit : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject rta_obj;
    public RazerToweAttack rta;

    void Start()
    {
        rta = rta_obj.GetComponent<RazerToweAttack>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        string requiredTag = rta.enemyTag;

        if (collision.CompareTag(requiredTag))
        {
            Debug.Log("플레이어가 레이저에 맞았습니다.");
            collision.GetComponent<PlayerMove>().pv.RPC("TakeDamage", RpcTarget.Others);
            collision.GetComponent<PlayerMove>().TakeDamage();
        }
    }
}
