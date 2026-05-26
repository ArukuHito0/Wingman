using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] private int takeDamage = 0;

    private Animator effectAnimator;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 5f;
    [SerializeField] private float flashSpeed = 2f;
    [SerializeField] private float minAlpha = 0.5f;
    [SerializeField] private bool useSmoothFlash = false;
    private Coroutine flashCoroutine;
    private Color originalColor;
    [SerializeField] private Collider2D playerCollider;

    [SerializeField] private Gradient healthGradient;   // インスペクターで色を指定
    [SerializeField] private Image healthFillImage;     // スライダーのFillオブジェクトを指定

    [SerializeField] private GameObject gameOverEffect;

    ShootingController shootingController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        effectAnimator = GetComponent<Animator>();
        gameOverEffect.SetActive(false);
        shootingController = GetComponent<ShootingController>();
        originalColor = spriteRenderer.color;
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

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(DamageFlashCoroutine());

        Debug.Log("現在の体力 : " + currentHealth);

        if (currentHealth <= 0)
        {
            GameOver();
        }

        UpdateHealthUI();
    }

    private IEnumerator DamageFlashCoroutine()
    {
        float elapsedTime = 0f;

        if (playerCollider != null)
        {
            playerCollider.enabled = false;
            if (shootingController  != null)
            {
                shootingController.IsShootingFalse();
            }
        }

        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            float targetAlpha = 1f;

            if (useSmoothFlash)
            {
                float pingPong = Mathf.PingPong(elapsedTime * flashSpeed, 1f - minAlpha);
                targetAlpha = minAlpha * pingPong;
            }
            else
            {
                if((int)(elapsedTime * flashSpeed) % 2 == 0)
                {
                    targetAlpha = minAlpha;
                }
                else
                {
                    targetAlpha = 1f;
                }
            }

            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);

            yield return null;
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = true;
            if (shootingController != null)
            {
                shootingController.IsShootingTrue();
            }
        }
        spriteRenderer.color = originalColor;
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