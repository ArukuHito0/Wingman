using UnityEngine;

public class PlanetHealth : MonoBehaviour
{
    private float currentHealth = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //衝突した相手のタグが"Bullet"の場合
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(10f);

            // 衝突した相手(collision)から BulletController スクリプトを取得
            BulletController bullet = collision.GetComponent<BulletController>();

            // 取得できた場合
            if (bullet != null)
            {
                bullet.ReturnToPool();  // 弾をPoolに戻す
            }
        }
    }
}