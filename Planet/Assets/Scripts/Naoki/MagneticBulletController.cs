using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class MagneticBulletController : MonoBehaviour
{
    public ObjectPool<GameObject> myPool;
    private IObjectPool<GameObject> hitEffectPool;
    public Rigidbody2D rb;

    [Header("弾丸設定")]
    private float shootSpeed = 20f;
    private Vector2 inheritedVelocity;  // プレイヤーの速度を受け取る変数

    // [UnitHeaderInspectable("破壊座標")]
    [Header("破壊設定")]
    // float destroyPositionY = 10;
    //public float lifeTime = 10;
    //private float timer = 0;

    private float activeAreaX = 8f;
    private float activeAreaY = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(0, shootSpeed * Time.deltaTime, 0);

        Vector3 moveStep = (transform.up * shootSpeed) + (Vector3)inheritedVelocity;
        transform.position += moveStep * Time.deltaTime;

        //timer += Time.deltaTime;

        //if(timer > lifeTime)
        //{
        //    // Destroy(gameObject);
        //    myPool.Release(gameObject);
        //}

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

    //public void SetInheritedVelocity(Vector2 velocity)
    //{
    //    inheritedVelocity = velocity;
    //}

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitEffectPool != null)
        {
            // プールからエフェクトを取得
            GameObject effect = hitEffectPool.Get();

            // エフェクトの位置を自分の位置に合わせる
            effect.transform.position = transform.position;
        }
        // 弾をプールに戻す
        ReturnToPool();
    }

    public void SetEffectPool(IObjectPool<GameObject> pool)
    {
        hitEffectPool = pool;
    }
}