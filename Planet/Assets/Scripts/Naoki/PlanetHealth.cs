using TMPro;
using UnityEngine;

public class PlanetHealth : MonoBehaviour
{
    [SerializeField] private float currentHealth = 30;
    [SerializeField] private int addScoreValue = 0;
    [SerializeField] private int minusScoreValue = 0;
    [SerializeField] private TextMeshPro healthText;

    private void OnEnable()
    {
        healthText.text = currentHealth.ToString("F0");
    }

    private void Awake()
    {
        healthText = transform.Find("HealthText").GetComponent<TextMeshPro>();
    }

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
        healthText.text = currentHealth.ToString("F0");
        if (currentHealth <= 0)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(addScoreValue);
            }
            else
            {
                Debug.LogWarning("ScoreManagerのインスタンスが存在しません！");
            }
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
                //bullet.ReturnToPool();  // 弾をPoolに戻す
            }
        }

        if (collision.CompareTag("Player"))
        {
            ScoreManager.Instance.MinusScore(minusScoreValue);
            Destroy(gameObject);
        }
    }
}