    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 6f;
    private Vector3 direction;
    private Rigidbody2D rigid;
    public void Initialize(Vector3 fireDirection)
    {
        direction = fireDirection.normalized;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rigid.AddForce(transform.up * speed);
    }
    private void Update()
    {
        //transform.position += direction * speed * Time.deltaTime;   
        

    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Enemy"))
    //     {
    //         
    //     }
    //     Destroy(gameObject);
    // }
}
