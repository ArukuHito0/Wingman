using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GaugeManager : MonoBehaviour
{
    public static GaugeManager Instance;

    [Header("スライダー設定")]
    public Slider mainGaugeSlider;       // 前面：実際のメインゲージ
    public Slider predictionGaugeSlider; // 背面：先行して増える予測ゲージ

    [Header("演出設定")]
    [SerializeField] private float fillSpeed = 30f; // メインゲージが追いつくスピード

    private float currentGauge = 0f;      // 実際のゲージ量
    private float predictedGauge = 0f;    // 予測も含めたゲージ量
    private float maxGauge = 100f;
    private bool isFull = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 両方のスライダーの最大値を合わせる
        mainGaugeSlider.maxValue = maxGauge;
        predictionGaugeSlider.maxValue = maxGauge;

        // 初期状態のゲージをUIに反映
        mainGaugeSlider.value = currentGauge;
        predictionGaugeSlider.value = currentGauge;
    }

    void Update()
    {
        // 必殺技発動中の減少処理（元の処理を維持）
        if (isFull)
        {
            currentGauge -= Time.deltaTime * 10f;
            predictedGauge = currentGauge; // 減少時は予測も同期させる

            if (currentGauge <= 0f)
            {
                ResetGauge();
            }

            mainGaugeSlider.value = currentGauge;
            predictionGaugeSlider.value = currentGauge;
            return;
        }

        // ★2連スライダーのコア処理: メインゲージを予測ゲージに向けて滑らかに追いつかせる
        if (mainGaugeSlider.value < currentGauge)
        {
            mainGaugeSlider.value = Mathf.MoveTowards(mainGaugeSlider.value, currentGauge, fillSpeed * Time.deltaTime);
        }
    }

    // 1. 【新設】オーブが触れた瞬間に、予測値だけを「先行して増やす」メソッド
    public void PredictGainGauge(float amount)
    {
        if (isFull) return;

        // 予測の数値を増やして、背面スライダーをパッと反映
        predictedGauge = Mathf.Clamp(predictedGauge + amount, 0f, maxGauge);
        predictionGaugeSlider.value = predictedGauge;
    }

    // 2. 【改造】オーブが到着したときに、実際のゲージ数値を増やすメソッド
    public void GainGauge(float amount)
    {
        if (isFull) return;

        currentGauge += amount;
        currentGauge = Mathf.Clamp(currentGauge, 0f, maxGauge);

        // 満タンになったかチェック
        if (currentGauge >= maxGauge)
        {
            TriggerSpecialAbility();
        }
    }

    // オーブの演出なしで、直接ゲージを増やすメソッド（他のオブジェクト用）
    public void AddGaugeDirect(float amount)
    {
        if (isFull) return;

        // 1. 背面の予測ゲージをパッと増やす
        PredictGainGauge(amount);

        // 2. 前面のメインゲージの目標値を増やす（これで自動的にUpdateで滑らかに追いつきます）
        GainGauge(amount);
    }

    // オーブの目的地（予測ゲージの先端）のワールド座標を計算して返すメソッド
    public Vector3 GetTargetWorldPosition()
    {
        // 予測ゲージ（背面のSlider）の現在の値の割合 (0.0 ～ 1.0)
        float ratio = predictionGaugeSlider.value / maxGauge;

        // Sliderのバーが伸びる部分（Fill Area）のRectTransformを取得
        RectTransform fillArea = predictionGaugeSlider.fillRect.parent as RectTransform;

        // Fill Area内での予測値の先端位置を計算
        Vector3 localPos = new Vector3(
            Mathf.Lerp(fillArea.rect.xMin, fillArea.rect.xMax, ratio),
            fillArea.rect.center.y,
            0f
        );

        // ローカル座標をワールド座標に変換して返す
        return fillArea.TransformPoint(localPos);
    }

    void TriggerSpecialAbility()
    {
        isFull = true;
        Debug.Log("ゲージ満タン！必殺技発動！");
        PlayerHealth.Instance.ActivateStarInvincible();
    }

    public void ResetGauge()
    {
        currentGauge = 0f;
        predictedGauge = 0f;
        mainGaugeSlider.value = currentGauge;
        predictionGaugeSlider.value = currentGauge;
        isFull = false;
    }
}