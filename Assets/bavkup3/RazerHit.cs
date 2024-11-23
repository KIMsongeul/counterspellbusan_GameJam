using System.Collections;
using System.Collections.Generic;
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
        if (!rta.isAttacking || rta.hasDealtDamage)
        {
            return;
        }

        string requiredTag = rta.Team == 0 ? "Team2" : "Team1";

        if (((1 << collision.gameObject.layer) & playerLayer) != 0 && collision.CompareTag(requiredTag))
        {
            Debug.Log("플레이어가 레이저에 맞았습니다.");
            rta.hasDealtDamage = true;
        }
    }
}
