using UnityEngine;
using UnityEngine.UI; // UIを扱うために必要

public class GaugeManager : MonoBehaviour
{
    public static GaugeManager Instance;
    public Slider gaugeSlider; // インスペクターでSliderを紐づけます

    private float currentGauge = 0f;
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
            Destroy(gameObject); // 重複防止
        }
    }

    void Start()
    {
        // 初期状態のゲージをUIに反映
        gaugeSlider.maxValue = maxGauge;
        gaugeSlider.value = currentGauge;
    }

    void Update()
    {
        if (isFull)
        {
            currentGauge -= Time.deltaTime * 10f;

            if (currentGauge <= 0f)
            {
                ResetGauge();
            }

            gaugeSlider.value = currentGauge;
        }
    }

    // ゲージを増やすメソッド（他のスクリプトから呼び出す）
    public void GainGauge(float amount)
    {
        if (isFull) return; // すでに満タンなら何もしない

        currentGauge += amount;

        // ゲージが最大値を超えないように制限
        currentGauge = Mathf.Clamp(currentGauge, 0f, maxGauge);

        // UIの値を更新
        gaugeSlider.value = currentGauge;

        // 満タンになったかチェック
        if (currentGauge >= maxGauge)
        {
            TriggerSpecialAbility();
        }
    }

    // 満タンになったときに発動する処理
    void TriggerSpecialAbility()
    {
        isFull = true;
        Debug.Log("ゲージ満タン！必殺技発動！");

        // 無敵状態にする処理
        PlayerHealth.Instance.ActivateStarInvincible();
    }



    // ゲージをリセットする処理（必要に応じて使う）
    public void ResetGauge()
    {
        currentGauge = 0f;
        gaugeSlider.value = currentGauge;
        isFull = false;
    }
}