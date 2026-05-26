using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class PlanetHealth : MonoBehaviour
{
    private Planet planet;

    [SerializeField] private float currentHealth = 30;
    [SerializeField] private int addScoreValue = 0;

    private int AddScoreValue()
    {
        switch (planet.attribute)
        {
            case PlanetAttribute.Normal:
                return addScoreValue;
            case PlanetAttribute.Rare:
                return addScoreValue * 2;
            default:
                return addScoreValue;
        }
    }

    private TextMeshPro healthText;

    [SerializeField] private TextMeshPro scoreTextPrefab;
    [SerializeField] private GameObject brokenEffect;
    [SerializeField] private GameObject explosionEffect;

    private void OnEnable()
    {
        healthText.text = currentHealth.ToString("F0");
    }

    private void Awake()
    {
        healthText = transform.Find("HealthText").GetComponent<TextMeshPro>();
        planet = GetComponent<Planet>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthText.text = currentHealth.ToString("F0");
        if (currentHealth <= 0)
        {
            AddScore();
            SpawnScoreText(AddScoreValue());
            Broken();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //衝突した相手のタグが"Bullet"の場合
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage(10);

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
            ScoreManager.Instance.AddScore(AddScoreValue() / 4);
            SpawnScoreText(AddScoreValue() / 4);
            Broken();
        }
    }

    /// <summary>
    /// 惑星を消去&破壊時のエフェクトをスポーン
    /// </summary>
    private void Broken()
    {
        if (planet.attribute == PlanetAttribute.Explosion)
        {
            if (explosionEffect != null)
            {
                AudioManager.instance.PlaySE("Explosion");

                Instantiate(
                    explosionEffect,
                    transform.position,
                    Quaternion.identity
                );
            }
        }

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
            ScoreManager.Instance.AddScore(AddScoreValue());
        }
        else
        {
            Debug.LogWarning("ScoreManagerのインスタンスが存在しません！");
        }
    }

    /// <summary>
    /// 破壊時に得られるスコアのテキストをスポーン
    /// </summary>
    private void SpawnScoreText(int score)
    {
        if (scoreTextPrefab != null)
        {
            TextMeshPro scoreText = Instantiate(
                scoreTextPrefab,
                transform.position,
                Quaternion.identity
            );

            if (planet.attribute == PlanetAttribute.Rare)
            {
                scoreText.text = $"<size={scoreText.fontSize * 0.6f}><color=yellow>x2 Bonus!</color></size>\n+ {score}!";
            }
            else
            {
                scoreText.text = $"+ {score}!";
            }
        }
    }
}