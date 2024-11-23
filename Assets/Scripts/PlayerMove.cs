using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    public float speed = 4;

    private Rigidbody2D rigid;
    [HideInInspector] public PhotonView pv;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Move()
    {
        if(!pv.IsMine) return;
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
    
