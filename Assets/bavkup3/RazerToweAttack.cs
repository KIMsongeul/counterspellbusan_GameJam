using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerToweAttack : MonoBehaviour
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

    public Transform[] attackTarget;

    void Update()
    {
        if(!NoRotation)
        {
            Transform target = attackTarget[Team];
            Vector2 direction = (target.position - transform.position).normalized;
            float degree = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, degree);
        }
    }

    void Start()
    {
        StartCoroutine(Rayzer_Start());
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
        Razer[0].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        NoRotation = true;
        yield return new WaitForSeconds(0.5f);
        Razer[0].SetActive(false);
        Razer[1].SetActive(true);

        isAttacking = true;
        hasDealtDamage = false;
        yield return new WaitForSeconds(duration);
        isAttacking = false;
        Razer[1].SetActive(false);
        NoRotation = false;
    }

}
