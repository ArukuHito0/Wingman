using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ShootingController : MonoBehaviour
{
    public Rigidbody2D playerRb;

    [Header("シューティング設定")]
    private float timer = 0f;

    [Header("シューティング設定")]
    public GameObject shootingObject;
    public Transform shootPoint;
    public float shootingSpeed = 1.0f;
    public float shootingCooltime = 0.25f;

    [Header("プールの宣言")]
    public IObjectPool<GameObject> bulletPool;

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= shootingCooltime)
        {
            bulletPool.Get();
            timer = 0f;
        }
    }

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
    }

    void OnReleaseBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    void OnDestroyBullet(GameObject bullet)
    {
        Destroy(bullet);
    }
}