using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletController : MonoBehaviour
{
    public ObjectPool<GameObject> myPool;
    public Rigidbody2D rb;

    [Header("弾丸設定")]
    private float shootSpeed = 5f;
    private Vector2 inheritedVelocity;  // プレイヤーの速度を受け取る変数

    // [UnitHeaderInspectable("破壊座標")]
    [Header("破壊座標")]
    // float destroyPositionY = 10;
    public float lifeTime = 5;
    private float timer = 0;

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

        timer += Time.deltaTime;

        if(timer > lifeTime)
        {
            // Destroy(gameObject);
            myPool.Release(gameObject);
        }
    }

    //public void SetInheritedVelocity(Vector2 velocity)
    //{
    //    inheritedVelocity = velocity;
    //}

    public void OnEnable()
    {
        timer = 0f; // タイマーをリセットして、また3秒数え直せるようにする
    }

    public void Launch(Vector2 inheritedVelocity)
    {
        // 自身のRigidbody2Dを取得（または事前にキャッシュしておく）
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // 前進速度 + プレイヤーの速度
        rb.linearVelocity = ((Vector2)transform.up * shootSpeed) + inheritedVelocity;
    }
}