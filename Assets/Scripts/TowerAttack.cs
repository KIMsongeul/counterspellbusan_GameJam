using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    private float attackRate = 1f;
    private float tempAttackRate = 0f;
    public float bulletSpeed = 6f;

    private float bulletSizeMultiplier = 1f;
    // private float bulletSpeedMultiplier = 1f;
    
    private Coroutine sizeCoroutine;
    // private Coroutine speedCoroutine;

    private SpriteRenderer sr;
    public PhotonView pv;

    public Transform attackTarget = null;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (attackTarget == null) {
            string enemyTag = PhotonNetwork.IsMasterClient ? "Red" : "Blue";
            attackTarget = GameObject.FindGameObjectWithTag(enemyTag).transform;
            AssignTarget(attackTarget);
            return;
        }
        
        UpdateAttackCooldown();
        TryAttack();
        RotateTowardsTarget();
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
    
    [PunRPC]
    public void SetBulletSizeMultiplier(float multiplier, float duration)
    {
        Debug.Log("총알배수대입1");
        if (sizeCoroutine != null) StopCoroutine(sizeCoroutine);
        sizeCoroutine = StartCoroutine(ApplyBulletSize(multiplier, duration));
    }
    
    private IEnumerator ApplyBulletSize(float multiplier, float duration)
    {
        Debug.Log("총알배수대입2");
        bulletSizeMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        bulletSizeMultiplier = 1f;
    }
    // public void SetBulletSpeedMultiplier(float multiplier, float duration)
    // {
    //     if (speedCoroutine != null) StopCoroutine(speedCoroutine);
    //     speedCoroutine = StartCoroutine(ApplyBulletSpeed(multiplier, duration));
    // }
    //
    // private IEnumerator ApplyBulletSpeed(float multiplier, float duration)
    // {
    //     bulletSpeed *= multiplier;
    //     yield return new WaitForSeconds(duration);
    //     bulletSpeed /= multiplier;
    // }

    public void AssignTarget(Transform target)
    {
        if (!pv.IsMine) return;
        
        PhotonView targetView = target.GetComponent<PhotonView>();
        if (targetView != null)
        {
            pv.RPC("SetTarget", RpcTarget.All, targetView.ViewID);
        }
    }

    private void UpdateAttackCooldown()
    {
        if (tempAttackRate > 0)
        {
            tempAttackRate -= Time.deltaTime;
        }
    }

    private void TryAttack()
    {
       
        if (tempAttackRate <= 0)
        {
            ShootBullet();
            tempAttackRate = attackRate;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector2 direction = attackTarget.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ShootBullet()
    {
        if (!pv.IsMine) return;
        
        Vector2 direction = (attackTarget.position - transform.position).normalized;
        GameObject bullet = PhotonNetwork.Instantiate("Prefabs/Bullet_Test", spawnPoint.position, Quaternion.identity);
        
        Debug.Log("총알크기배수 : " + bulletSizeMultiplier);
        bullet.transform.localScale *= bulletSizeMultiplier;
        
        Debug.Log($"총알 생성: {bullet.name} at {spawnPoint.position}");
        
        TurretBullet bulletComponent = bullet.GetComponent<TurretBullet>();
        if (bulletComponent != null)
        {
            bulletComponent.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, direction);
            
            Color towerColor = sr.color;
            bulletComponent.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.All, towerColor.r, towerColor.g, towerColor.b);
        }
    }
    
    
    

    [PunRPC]
    public void SetColor(float r, float g, float b)
    {
        if (sr != null)
        {
            sr.color = new Color(r, g, b);
        }
    }
}