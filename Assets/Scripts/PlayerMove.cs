using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 4;

    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Move()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        
        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;
        
        rigid.velocity = moveDirection * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();
    }
}
    
