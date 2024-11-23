using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{
    public float speed = 4;

    public int Money = 0;

    public int HP = 3;

    private Rigidbody2D rigid;
    GameManager gameManager;
    [HideInInspector] public PhotonView pv;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        pv = GetComponent<PhotonView>();
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(AddMoney());
    }

    IEnumerator AddMoney(){
        while(true){
            if(!gameManager.isGameInProgress){
                yield return null;
                continue;
            }
            yield return new WaitForSeconds(1f);
            Money += 100;
        }
    }

    void Move()
    {
        if(!pv.IsMine) return;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        
        Vector2 moveDirection = new Vector2(horizontal, vertical).normalized;
        
        rigid.velocity = moveDirection * speed;
    }

    private void FixedUpdate()
    {
        Move();
    }

    [PunRPC]
    public void SetColor(float r, float g, float b)
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = new Color(r, g, b);
    }

    public void TakeDamage(){
        if(HP <= 1){
            Debug.Log(gameManager == null);
            Debug.Log(gameManager.pv == null);
            gameManager.pv.RPC("GameOver", RpcTarget.All);
            Debug.Log("Game Over");
            return;
        }
        HP -= 1;
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Tower");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach(GameObject turret in turrets){
            PhotonNetwork.Destroy(turret);
        }
        foreach(GameObject bullet in bullets){
            PhotonNetwork.Destroy(bullet);
        }
    }
}
    
