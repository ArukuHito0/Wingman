using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ShootingController : MonoBehaviour
{
    public Rigidbody2D playerRb;

    [Header("シューティング設定")]
    private float shootingTimer = 0f;
    private float magneticTimer = 0f;

    [Header("シューティング設定")]
    public GameObject shootingObject;
    public GameObject magneticShootingObject;
    public Transform shootPoint;
    public float shootingSpeed = 1.0f;
    public float shootingCooltime = 0.25f;
    public float magneticCooltime = 3f;

    private bool isShooting = true;

    [SerializeField] private GameObject hitEffectPrefab;

    [Header("プールの宣言")]
    public IObjectPool<GameObject> bulletPool;
    public IObjectPool<GameObject> magneticBulletPool;
    public IObjectPool<GameObject> hitEffectPool;

    [Header("参照")]
    ShootingController shootingController;
    PlayerController playerController;
    BulletController bulletController;
    MagneticBulletController magneticBulletController;

    void Awake()
    {
        bulletPool = new ObjectPool<GameObject>
        (
            CreateBullet,       // 作成時のメソッド
            OnGetBullet,        // 取り出すときのメソッド
            OnReleaseBullet,    // 戻すときのメソッド
            OnDestroyBullet     // 破棄時のメソッド
        );

        magneticBulletPool = new ObjectPool<GameObject>
        (
            CreateMagneticBullet,       // 作成時のメソッド
            OnGetMagneticBullet,        // 取り出すときのメソッド
            OnReleaseMagneticBullet,    // 戻すときのメソッド
            OnDestroyMagneticBullet     // 破棄時のメソッド
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
            magneticTimer += Time.deltaTime;
            if (shootingTimer >= shootingCooltime)
            {
                bulletPool.Get();
                shootingTimer = 0f;
            }

            if (magneticTimer >= magneticCooltime)
            {
                if (Input.GetMouseButton(1))
                {
                    magneticBulletPool.Get();
                    magneticTimer = 0f;
                }
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

    // MagneticBullet
    GameObject CreateMagneticBullet()
    {
        GameObject magneticBullet = Instantiate(magneticShootingObject);
        // 弾のスクリプトを取得して、プールにセットする
        magneticBullet.GetComponent<MagneticBulletController>().myPool = (ObjectPool<GameObject>)magneticBulletPool;
        return magneticBullet;
    }

    void OnGetMagneticBullet(GameObject magneticBullet)
    {
        magneticBullet.SetActive(true);
        magneticBullet.transform.position = shootPoint.position;
        magneticBullet.transform.rotation = shootPoint.rotation;

        Vector2 playerVelocity = playerRb.linearVelocity;

        magneticBullet.GetComponent<MagneticBulletController>().Launch(playerVelocity);
    }

    void OnReleaseMagneticBullet(GameObject magneticBullet)
    {
        magneticBullet.SetActive(false);
    }

    void OnDestroyMagneticBullet(GameObject magneticBullet)
    {
        Destroy(magneticBullet);
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