using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class MagneticBulletController : MonoBehaviour
{
    public ObjectPool<GameObject> myPool;
    public Rigidbody2D rb;

    [Header("弾丸設定")]
    [SerializeField] private float shootSpeed = 20f;
    private Vector2 inheritedVelocity;  // プレイヤーの速度を受け取る変数

    private float activeAreaX = 8f;
    private float activeAreaY = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 moveStep = (transform.up * shootSpeed) + (Vector3)inheritedVelocity;
        
        // Vector3 moveStep = (transform.up * shootSpeed); // プレイヤーの速度を追加しない
        // transform.position += moveStep * Time.deltaTime;

        if (transform.position.x > activeAreaX || transform.position.x < -activeAreaX)
        {
            myPool.Release(gameObject);
        }
        else if (transform.position.y > activeAreaY || transform.position.y < -activeAreaY)
        {
            myPool.Release(gameObject);
        }
    }

    public void ReturnToPool()
    {
        if (myPool != null)
        {
            myPool.Release(gameObject);
        }
    }

    public void OnEnable()
    {
        //timer = 0f; // タイマーをリセットして、また3秒数え直せるようにする
    }

    public void Launch(Vector2 inheritedVelocity)
    {
        // 自身のRigidbody2Dを取得（または事前にキャッシュしておく）
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // 前進速度 + プレイヤーの速度
        rb.linearVelocity = ((Vector2)transform.up * shootSpeed) + inheritedVelocity;
    }

    public void Fire()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }
}