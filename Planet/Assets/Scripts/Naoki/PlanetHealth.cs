using TMPro;
using UnityEngine;

public class PlanetHealth : MonoBehaviour
{
    [SerializeField] private float currentHealth = 30;
    [SerializeField] private int addScoreValue = 0;
    [SerializeField] private int minusScoreValue = 0;
    private TextMeshPro healthText;

    [SerializeField] private TextMeshPro scoreTextPrefab;
    [SerializeField] private GameObject brokenEffect;

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
            AddScore();
            SpawnScoreText();
            Broken();
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
            Broken();
        }
    }

    /// <summary>
    /// 惑星を消去&破壊時のエフェクトをスポーン
    /// </summary>
    private void Broken()
    {
        SpawnBrokenEffect();
        Destroy(gameObject);
    }

    /// <summary>
    /// 破壊時のエフェクトをスポーンさせる
    /// </summary>
    private void SpawnBrokenEffect()
    {
        if (brokenEffect != null)
        {
            Instantiate(
                brokenEffect,
                transform.position,
                Quaternion.identity
            );
        }
    }

    /// <summary>
    /// スコアマネージャーにスコアを加算する
    /// </summary>
    private void AddScore()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(addScoreValue);
        }
        else
        {
            Debug.LogWarning("ScoreManagerのインスタンスが存在しません！");
        }
    }

    /// <summary>
    /// 破壊時に得られるスコアのテキストをスポーン
    /// </summary>
    private void SpawnScoreText()
    {
        if (scoreTextPrefab != null)
        {
            TextMeshPro scoreText = Instantiate(
                scoreTextPrefab,
                transform.position,
                Quaternion.identity
            );

            scoreText.text = $"+ {addScoreValue}!";
        }
    }
}