using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    private float attackRate = 1f;
    private float tempAttackRate = 0f;

    private Transform attackTarget = null;


    private void Awake()
    {
        attackTarget = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if (attackTarget == null) return;
        
        UpdateAttackCooldown();
        TryAttack();
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
            RotateTowardsTarget();
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
        Vector2 direction = attackTarget.position - transform.position;
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, transform.rotation);
        bullet.GetComponent<Bullet>().Initialize(direction);
    }
}