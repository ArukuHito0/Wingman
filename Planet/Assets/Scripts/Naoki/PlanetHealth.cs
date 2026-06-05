using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetHealth : MonoBehaviour
{
    private Planet planet;

    [SerializeField] private float currentHealth = 30;
    [SerializeField] private int addScoreValue = 0;

    private TextMeshPro healthText;

    [SerializeField] private TextMeshPro scoreTextPrefab;
    [SerializeField] private TextMeshPro feverScoreTextPrefab;
    [SerializeField] private GameObject brokenEffect;
    [SerializeField] private GameObject feverBrokenEffect;
    [SerializeField] private GameObject explosionEffect;

    private string currentSceneName = string.Empty;

    private void OnEnable()
    {
        if(healthText != null)
            healthText.text = currentHealth.ToString("F0");
    }

    private void Awake()
    {
        healthText = transform.Find("HealthText")?.GetComponent<TextMeshPro>();
        planet = GetComponent<Planet>();

        currentSceneName = SceneManager.GetActiveScene().name;
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
        }

        if (collision.CompareTag("Player"))
        {
            if (PlayerHealth.Instance != null && PlayerHealth.Instance.isStarInvincible == true)
            {
                if (currentSceneName != "Title")
                {
                    ScoreManager.Instance?.AddScore(AddScoreValue());
                    SpawnScoreText(AddScoreValue());
                }

                Broken();
            }
            else if (PlayerHealth.Instance != null && PlayerHealth.Instance.isStarInvincible == false)
            {
                if (currentSceneName != "Title")
                {
                    ScoreManager.Instance?.AddScore(AddScoreValue() / 4);
                    SpawnScoreText(AddScoreValue() / 4);
                }

                Broken();
            }
        }
    }

    /// <summary>
    /// 惑星を消去&破壊時のエフェクトをスポーン
    /// </summary>
    private void Broken()
    {
        if (PlayerHealth.Instance.isStarInvincible)
        {
            SpacePlanetSpawner.Instance?.AutoSpawnPlanet();
        }

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

        AudioManager.instance.PlaySE("Broken");
        SpawnBrokenEffect();
        Destroy(gameObject);
    }

    /// <summary>
    /// 破壊時のエフェクトをスポーンさせる
    /// </summary>
    private void SpawnBrokenEffect()
    {
        if (PlayerHealth.Instance.isStarInvincible)
        {
            if (feverBrokenEffect != null)
            {
                Instantiate(
                    feverBrokenEffect,
                    transform.position,
                    Quaternion.identity
                );
            }
        }
        else
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

    private int AddScoreValue()
    {
        if (PlayerHealth.Instance != null && PlayerHealth.Instance.isStarInvincible == true)
        {
            switch (planet.attribute)
            {
                case PlanetAttribute.Normal:
                    return (addScoreValue) * 2;
                case PlanetAttribute.Rare:
                    return (addScoreValue * 2) * 2;
                default:
                    return (addScoreValue) * 2;
            }
        }
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

    /// <summary>
    /// 破壊時に得られるスコアのテキストをスポーン
    /// </summary>
    private void SpawnScoreText(int score)
    {
        if (PlayerHealth.Instance.isStarInvincible)
        {
            if (feverScoreTextPrefab != null)
            {
                TextMeshPro feverScoreText = Instantiate(
                feverScoreTextPrefab,
                transform.position,
                Quaternion.identity
                );

                feverScoreText.text = $"x2 FEVER!\n+ {score}!";
            }
        }
        else
        {
            if (scoreTextPrefab != null)
            {
                TextMeshPro scoreText = Instantiate(
                    scoreTextPrefab,
                    transform.position,
                    Quaternion.identity
                );

                if (planet.attribute == PlanetAttribute.Rare && !PlayerHealth.Instance.isStarInvincible)
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
}