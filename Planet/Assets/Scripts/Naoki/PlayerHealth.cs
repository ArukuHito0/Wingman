using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

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

    [Header("Star Invincible Settings")]
    [SerializeField] private float starInvincibleDuration = 10f; // スターの無敵時間
    [SerializeField] private GameObject starInvincibleEffect;       // 無敵時のエフェクト
    [SerializeField] private SpriteRenderer starInvinciblePlayerSprite;
    [SerializeField] private SpriteRenderer normalPlayerSprite;

    MagnetSkill magnetSkill;

    private Coroutine starInvincibleCoroutine;
    public bool isStarInvincible = false;

    [Header("Invincible Overlay Visual Settings")]
    [SerializeField] private Image invincibleOverlayImage;
    [SerializeField] private float maxOverlayAlpha = 0.8f;
    private Coroutine overlayCoroutine;

    public bool isFlashing = false;

    void Awake()
    {
        Instance = this;
        effectAnimator = GetComponent<Animator>();
        gameOverEffect.SetActive(false);
        shootingController = GetComponent<ShootingController>();
        magnetSkill = GetComponent<MagnetSkill>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        SetOverlayAlpha(0f);
    }
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        UpdateHealthUI();
        SetOverlayAlpha(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (starInvincibleEffect != null)
        {
            if (isStarInvincible == true)
            {
                starInvincibleEffect.SetActive(true);
            }
            else if (isStarInvincible == false)
            {
                starInvincibleEffect.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Planet"))
        {
            // スター状態でなければダメージを受ける
            if (!isStarInvincible && !isFlashing)
            {
                TakeDamage();
            }
        }
    }

    /// <summary>
    /// ゲージ満タン時やアイテム取得時に、外部から無敵状態を起動するための関数
    /// </summary>
    public void ActivateStarInvincible()
    {
        // 既にスター状態なら一度止める
        if (starInvincibleCoroutine != null)
        {
            StopCoroutine(starInvincibleCoroutine);
        }
        if (overlayCoroutine != null)
        {
            StopCoroutine(overlayCoroutine);
        }

        // スター状態のコルーチンをスタート
        starInvincibleCoroutine = StartCoroutine(StarInvincibleCoroutine());
    }

    public void TakeDamage()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }
        flashCoroutine = StartCoroutine(DamageFlashCoroutine());


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
            isFlashing = true;
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
            isFlashing = false;
            if (shootingController != null)
            {
                shootingController.IsShootingTrue();
            }
        }
        spriteRenderer.color = originalColor;
    }

    //無敵上タオ
    private IEnumerator StarInvincibleCoroutine()
    {
        isStarInvincible = true;
        
        AudioManager.instance.StopBGM();
        yield return null;
        AudioManager.instance.PlayBGM("Invincible");

        StartInvincibleOverlay();

        yield return new WaitForSeconds(starInvincibleDuration);
        isStarInvincible = false;

        AudioManager.instance.StopBGM();
        yield return null;
        AudioManager.instance.PlayBGM("Shooting");

        StopInvincibleOverlay();
    }

    private void StartInvincibleOverlay()
    {
        // 既に演出が動いていたら一度停止
        if (overlayCoroutine != null)
        {
            StopCoroutine(overlayCoroutine);
        }

        // 演出コルーチンを開始
        overlayCoroutine = StartCoroutine(FadeInvincibleOverlayCoroutine());
    }

    private void StopInvincibleOverlay()
    {
        if (overlayCoroutine != null)
        {
            StopCoroutine(overlayCoroutine);
        }
        SetOverlayAlpha(0f);
    }

    private IEnumerator FadeInvincibleOverlayCoroutine()
    {
        float elapsedTime = 0f;
        float fadeTime = 0.5f;
        float warningTime = 3.0f;
        float blinkSpeed = 15f;
        //float fadeInDuration = 0.5f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, maxOverlayAlpha, elapsedTime / fadeTime);
            SetOverlayAlpha(alpha);
            yield return null;
        }

        float maintainTime = starInvincibleDuration - fadeTime - warningTime;
        elapsedTime = 0f;
        while (elapsedTime < maintainTime)
        {
            elapsedTime += Time.deltaTime;

            SetOverlayAlpha(maxOverlayAlpha);


            yield return null;
        }

        elapsedTime = 0f;

        float lastAlphaBeforeFade = maxOverlayAlpha;

        while (elapsedTime < warningTime)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < (warningTime - fadeTime))
            {
                float blinkWave = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
                float alpha = blinkWave * maxOverlayAlpha;
                SetOverlayAlpha(alpha);
                lastAlphaBeforeFade = alpha;
            }
            else
            {
                float fadeElapsedTime = elapsedTime - (warningTime - fadeTime);

                float fadeOutProgress = fadeElapsedTime / fadeTime;

                float alpha = Mathf.Lerp(lastAlphaBeforeFade, 0f, fadeOutProgress);
                SetOverlayAlpha(alpha);
            }

            yield return null;
        }

        SetOverlayAlpha(0f);
    }

    private void SetOverlayAlpha(float alpha)
    {
        if (invincibleOverlayImage != null)
        {
            Color color = invincibleOverlayImage.color;
            color.a = alpha;
            invincibleOverlayImage.color = color;
        }

        SetInvincibleVisualAlpha(alpha);
    }

    private void SetNormalVisualAlpha(float alpha)
    {
        if (normalPlayerSprite != null)
        {
            Color color = normalPlayerSprite.color;
            color.a = alpha;
            normalPlayerSprite.color = color;
        }
    }

    private void SetInvincibleVisualAlpha(float alpha)
    {
        if (starInvinciblePlayerSprite != null)
        {
            Color color = starInvinciblePlayerSprite.color;
            color.a = alpha;
            starInvinciblePlayerSprite.color = color;
        }
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