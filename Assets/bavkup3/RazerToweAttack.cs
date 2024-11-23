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
            StartCoroutine(Rayzer_shut(targetScale, duration));
        }
    }

    IEnumerator Rayzer_shut(float targetXScale, float duration)
    {
        Razer[0].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        NoRotation = true;
        yield return new WaitForSeconds(0.5f);
        Razer[0].SetActive(false);
        Razer[1].SetActive(true);

        isAttacking = true;
        hasDealtDamage = false;

        Vector3 initialScale = Razer[1].transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newScaleX = Mathf.Lerp(initialScale.x, targetXScale, elapsedTime / duration);
            Razer[1].transform.localScale = new Vector3(newScaleX, initialScale.y, initialScale.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Razer[1].transform.localScale = new Vector3(targetXScale, initialScale.y, initialScale.z);
        isAttacking = false;
        Razer[1].SetActive(false);
        NoRotation = false;
    }

}
