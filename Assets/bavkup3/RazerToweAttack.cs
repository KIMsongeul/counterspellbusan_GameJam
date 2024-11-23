using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RazerToweAttack : MonoBehaviourPunCallbacks
{
    public GameObject[] Razer;
    public Transform spawnPoint;
    public float AttackReloadMax = 16;
    public float AttackReloadMin = 5;
    public int Team;

    public bool isAttacking = false;
    public bool hasDealtDamage = false;
    public bool NoRotation = false;

    public float targetScale = 2.5f;
    public float duration = 0.5f;

    public Transform attackTarget;
    private PhotonView pv;
    public string enemyTag = "Red";

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        if(pv.IsMine) {
            // 팀 설정
            Team = PhotonNetwork.IsMasterClient ? 0 : 1;
            enemyTag = Team == 0 ? "Red" : "Blue";
        }
    }

    private void Update()
    {
        if(!pv.IsMine) return;

        if (attackTarget == null) {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            if(enemies.Length > 0) {
                float nearestDistance = float.MaxValue;
                GameObject nearestEnemy = null;
                
                foreach(GameObject enemy in enemies) {
                    float distance = Vector2.Distance(transform.position, enemy.transform.position);
                    if(distance < nearestDistance) {
                        nearestDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
                
                if(nearestEnemy != null) {
                    attackTarget = nearestEnemy.transform;
                    AssignTarget(attackTarget);
                }
            }
            return;
        }

        if(!NoRotation)
        {
            Vector2 direction = (attackTarget.position - transform.position).normalized;
            float degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, degree);
        }
    }

    void Start()
    {
        if(pv.IsMine)
        {
            StartCoroutine(Rayzer_Start());
        }
    }

    [PunRPC]
    public void SetTarget(int targetViewID)
    {
        PhotonView targetView = PhotonView.Find(targetViewID);
        if (targetView != null)
        {
            attackTarget = targetView.transform;
        }
    }

    public void AssignTarget(Transform target)
    {
        if (!pv.IsMine) return;
        
        PhotonView targetView = target.GetComponent<PhotonView>();
        if (targetView != null)
        {
            pv.RPC("SetTarget", RpcTarget.All, targetView.ViewID);
        }
    }

    IEnumerator Rayzer_Start()
    {
        while (true)
        {
            float waitTime = Random.Range(AttackReloadMin, AttackReloadMax);
            yield return new WaitForSeconds(waitTime);
            StartCoroutine(Rayzer_shut(duration));
        }
    }

    IEnumerator Rayzer_shut(float duration)
    {
        pv.RPC("SetRazerState", RpcTarget.All, 0, true);
        yield return new WaitForSeconds(1.5f);
        pv.RPC("SetNoRotation", RpcTarget.All, true);
        yield return new WaitForSeconds(0.5f);
        pv.RPC("SetRazerState", RpcTarget.All, 0, false);
        pv.RPC("SetRazerState", RpcTarget.All, 1, true);

        pv.RPC("SetAttackState", RpcTarget.All, true);
        yield return new WaitForSeconds(duration);
        pv.RPC("SetAttackState", RpcTarget.All, false);
        pv.RPC("SetRazerState", RpcTarget.All, 1, false);
        pv.RPC("SetNoRotation", RpcTarget.All, false);
    }

    [PunRPC]
    void SetRazerState(int index, bool state)
    {
        Razer[index].SetActive(state);
    }

    [PunRPC]
    void SetNoRotation(bool state)
    {
        NoRotation = state;
    }

    [PunRPC]
    void SetAttackState(bool state)
    {
        isAttacking = state;
        if(state)
        {
            hasDealtDamage = false;
        }
    }

    [PunRPC]
    public void SetColor(float r, float g, float b)
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(r, g, b);
        }

        SpriteRenderer sr2 = transform.GetChild(2).GetComponent<SpriteRenderer>();
        if (sr2 != null)
        {
            sr2.color = new Color(r, g, b);
        }
    }   
}
