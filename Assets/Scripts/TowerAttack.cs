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
    private bool isMultiShotActive = true;
    // private float bulletSpeedMultiplier = 1f;
    
    private Coroutine sizeCoroutine;
    private Coroutine multiShotCoroutine;

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

        // 기본 총알 발사
        FireBullet(direction);

        // 멀티샷이 활성화되었으면 추가 총알 발사
        if (isMultiShotActive)
        {
            FireAdditionalShots(direction);
        }
    }
        private void FireBullet(Vector2 direction)
        {
            GameObject bullet = PhotonNetwork.Instantiate("Prefabs/Bullet_Test", spawnPoint.position, Quaternion.identity);
            TurretBullet bulletComponent = bullet.GetComponent<TurretBullet>();

            if (bulletComponent != null)
            {
                bulletComponent.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, direction);
                bullet.transform.localScale *= bulletSizeMultiplier; // 총알 크기 배율 적용
            }
        }
        
        
        
        private void FireAdditionalShots(Vector2 direction)
        {
            float angleOffset = 10f;
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0) continue;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + (angleOffset * i);
                Vector2 newDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                FireBullet(newDirection);
            }
        }
        
        
        [PunRPC]
        public void ActivateMultiShot(float duration)
        {
            if (multiShotCoroutine != null) StopCoroutine(multiShotCoroutine);
            multiShotCoroutine = StartCoroutine(ApplyMultiShot(duration));
        }

    private IEnumerator ApplyMultiShot(float duration)
    {
            isMultiShotActive = true;
            yield return new WaitForSeconds(duration);
            isMultiShotActive = false;
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
    
    
    

