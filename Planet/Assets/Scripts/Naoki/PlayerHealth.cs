using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private int takeDamage = 0;

    private Animator effectAnimator;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Gradient healthGradient;   // インスペクターで色を指定
    [SerializeField] private Image healthFillImage;     // スライダーのFillオブジェクトを指定

    [SerializeField] private GameObject gameOverEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        effectAnimator = GetComponent<Animator>();
        gameOverEffect.SetActive(false);
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        UpdateHealthUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            TakeDamage(takeDamage);

            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("現在の体力 : " + currentHealth);

        if (currentHealth <= 0)
        {
            GameOver();
        }

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        // 体力の割合を計算
        float healthNormalized = (float)currentHealth / maxHealth;

        // グラデーションから色を取得して色を更新
        healthFillImage.color = healthGradient.Evaluate(healthNormalized);

        // 体力表示
        string healthString = currentHealth.ToString().PadLeft(3);
        healthText.text = "HP : <mspace=20>" + healthString + "</mspace> / 100";

        // スライダー更新
        healthSlider.value = currentHealth;
    }

    void GameOver()
    {
        Debug.Log("GameOver");

        if (gameOverEffect != null)
        {
            // 親子関係を解除
            gameOverEffect.transform.SetParent(null);

            // エフェクト表示
            gameOverEffect.SetActive(true);

            // エフェクトのAnimatorではじめから再生
            Animator anim = gameOverEffect.GetComponent<Animator>();
            if (anim != null)
            {
                anim.Play("hits-2-1 (1)", 0, 0.0f);
            }
        }
        // プレイヤーを破壊
        Destroy(gameObject);
    }

    public void OnAnimationComplete()
    {
        gameOverEffect.SetActive(false);
    }
}