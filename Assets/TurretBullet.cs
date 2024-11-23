using UnityEngine;
using Photon.Pun;

public class TurretBullet : MonoBehaviourPunCallbacks
{
    public float speed = 10f;
    private Rigidbody2D rb;
    private PhotonView pv;
    private SpriteRenderer sr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
        sr = GetComponent<SpriteRenderer>();
        
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer가 없습니다!");
        }
    }

    [PunRPC]
    public void Initialize(Vector2 dir)
    {
        if (rb == null) return;
        
        Vector2 direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        rb.velocity = direction * speed;
        
        Debug.Log($"총알 초기화: 속도={rb.velocity}, 위치={transform.position}");
    }

    [PunRPC]
    public void SetColor(float r, float g, float b)
    {
        if (sr != null)
        {
            sr.color = new Color(r, g, b);
        }
    }

    [PunRPC]
    public void DestroyBullet(){
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Red") || other.CompareTag("Blue"))
        {
            Color bulletColor = sr.color;
            bool isBlue = Mathf.Approximately(bulletColor.b, 1f);
            
            if ((isBlue && other.CompareTag("Red")) || (!isBlue && other.CompareTag("Blue")))
            {
                Debug.Log("적중!");
                DestroyBullet();
            }
        }
    }
}
