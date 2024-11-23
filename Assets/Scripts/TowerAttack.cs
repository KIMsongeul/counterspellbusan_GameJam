using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget}
public class TowerAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    private float attackRate = 1f;
    private float tempAttackRate = 0f;
    private float attackRange = 10f;

    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;


    private void Awake()
    {
        attackTarget = GameObject.Find("Player").transform;
    }

    public void Setup()
    {
        
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        StopCoroutine(weaponState.ToString());

        weaponState = newState;
        StopCoroutine(weaponState.ToString());
    }

    private void Update()
    {
        if (attackTarget != null)
        {
            RatateToTarget(attackTarget);
        }

        if (tempAttackRate > 0)
        {
            tempAttackRate -= Time.deltaTime;
        }
        else
        {
            Attack();
            tempAttackRate = attackRate;
        }
    }

    public void RatateToTarget(Transform target)
    {
        float dx = target.position.x - transform.position.x;
        float dy = target.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,degree);
    }

    private void Attack()
    {
        if (attackTarget == null)
        {
            ChangeState(WeaponState.SearchTarget);
            return;
        }

        if (Vector3.Distance(attackTarget.position, transform.position) > attackRange)
        {
            attackTarget = null;
            ChangeState(WeaponState.SearchTarget);
            return;
        }
        
        SpawnBullet();
    }

    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            yield return new WaitForSeconds(attackRate);
            SpawnBullet();
        }
    }

    public void SpawnBullet()
    {
        
    }
    

}