using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ShootingController : MonoBehaviour
{
    public Rigidbody2D playerRb;

    [Header("シューティング設定")]
    private float shootingTimer = 0f;

    [Header("シューティング設定")]
    public GameObject shootingObject;
    public GameObject gravityHolePrefab;
    public Transform shootPoint;
    public float shootingSpeed = 1.0f;
    public float shootingCooltime = 0.25f;

    private bool isShooting = true;

    [SerializeField] private GameObject hitEffectPrefab;

    [Header("プールの宣言")]
    public IObjectPool<GameObject> bulletPool;
    public IObjectPool<GameObject> gravityHolePool;
    public IObjectPool<GameObject> hitEffectPool;

    [Header("参照")]
    ShootingController shootingController;
    PlayerController playerController;
    BulletController bulletController;

    void Awake()
    {
        bulletPool = new ObjectPool<GameObject>
        (
            CreateBullet,       // 作成時のメソッド
            OnGetBullet,        // 取り出すときのメソッド
            OnReleaseBullet,    // 戻すときのメソッド
            OnDestroyBullet     // 破棄時のメソッド
        );

        gravityHolePool = new ObjectPool<GameObject>
        (
            CreateGravityHole,       // 作成時のメソッド
            OnGetGravityHole,        // 取り出すときのメソッド
            OnReleaseGravityHole,    // 戻すときのメソッド
            OnDestroyGravityHole     // 破棄時のメソッド
        );

        hitEffectPool = new ObjectPool<GameObject>
        (
            CreateHitEffect,       // 作成時のメソッド
            OnGetHitEffect,        // 取り出すときのメソッド
            OnReleaseHitEffect,    // 戻すときのメソッド
            OnDestroyHitEffect     // 破棄時のメソッド
        );
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting == true)
        {
            shootingTimer += Time.deltaTime;
            if (shootingTimer >= shootingCooltime)
            {
                bulletPool.Get();
                shootingTimer = 0f;
            }

            if (Input.GetMouseButtonDown(1))
            {
                GameObject obj = gravityHolePool.Get();
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                obj.transform.position = pos;
            }
        }
    }

    public void IsShootingTrue()
    {
        isShooting = true;
    }

    public void IsShootingFalse()
    {
        isShooting = false;
    }

    // Bullet
    GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(shootingObject);
        // 弾のスクリプトを取得して、プールにセットする
        bullet.GetComponent<BulletController>().myPool = (ObjectPool<GameObject>)bulletPool;
        return bullet;
    }

    void OnGetBullet(GameObject bullet)
    {
        bullet.SetActive(true);
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = shootPoint.rotation;

        Vector2 playerVelocity = playerRb.linearVelocity;

        bullet.GetComponent<BulletController>().Launch(playerVelocity);
        bullet.GetComponent<BulletController>().SetEffectPool (hitEffectPool);
    }

    void OnReleaseBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    void OnDestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }

    // GravityHole
    GameObject CreateGravityHole()
    {
        GameObject gravityHole = Instantiate(gravityHolePrefab, transform.position, Quaternion.identity);
        // 弾のスクリプトを取得して、プールにセットする
        gravityHole.GetComponent<GravityHole>().myPool = (ObjectPool<GameObject>)gravityHolePool;
        return gravityHole;
    }

    void OnGetGravityHole(GameObject gravityHole)
    {
        gravityHole.SetActive(true);
    }

    void OnReleaseGravityHole(GameObject gravityHole)
    {
        gravityHole.SetActive(false);
    }

    void OnDestroyGravityHole(GameObject gravityHole)
    {
        Destroy(gravityHole);
    }

    GameObject CreateHitEffect()
    {
        // オブジェクトを生成
        GameObject effect = Instantiate(hitEffectPrefab);

        // 生成したエフェクトに変える先のプールを教える
        effect.GetComponent<HitEffectController>().SetPool(hitEffectPool);

        // 生成したオブジェクトを返す
        return effect;
    }

    void OnGetHitEffect(GameObject effect)
    {
        // アクティブにする処理
        effect.SetActive(true);
    }

    void OnReleaseHitEffect(GameObject effect)
    {
        // 非アクティブにする処理
        effect.SetActive(false);
    }

    void OnDestroyHitEffect(GameObject effect)
    {
        Destroy(effect);
    }
}