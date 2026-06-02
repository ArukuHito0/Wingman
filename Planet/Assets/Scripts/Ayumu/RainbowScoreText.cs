using System.Collections;
using TMPro;
using UnityEngine;

public class RainbowScoreText : MonoBehaviour
{
    private TextMeshPro scoreText;

    [Header("シーソー（回転）の設定")]
    [Tooltip("揺れる最大の角度")]
    public float maxRotationAngle = 20f;
    [Tooltip("揺れるスピード")]
    public float rotationSpeed = 15f;

    [Header("拡縮（スケール）の設定")]
    [Tooltip("基準となる基本サイズ")]
    public Vector3 baseScale = Vector3.one;
    [Tooltip("拡大縮小の振り幅（0.2なら基本サイズ±0.2）")]
    public float scaleAmplitude = 0.2f;
    [Tooltip("拡縮のスピード")]
    public float scaleSpeed = 20f;

    [Header("消滅の設定")]
    [SerializeField] private float animationTime = 2.0f;
    [Tooltip("消滅に向けて縮小を開始するタイミング（割合：0.7なら全体の7割が経過したら縮小開始）")]
    [Range(0f, 1f)]
    [SerializeField] private float fadeOutStartRatio = 0.7f;

    [Header("虹色エフェクトの設定")]
    public float rainbowSpeed = 0.5f;

    private RectTransform rectTransform;

    void Start()
    {
        scoreText = GetComponent<TextMeshPro>();
        rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
        {
            Debug.LogError("このスクリプトはUI（Text等）のGameObjectにアタッチしてください。");
            return; // 早期リターンしてコルーチン起動を防ぐ
        }

        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        float time = 0;

        while (time < animationTime)
        {
            if (rectTransform == null) yield break;

            // 現在の時間の割合（0.0 〜 1.0）
            float progressRatio = time / animationTime;

            // --- 1. 消滅用の全体スケール倍率の計算 ---
            float fadeScaleMultiplier = 1.0f;

            if (progressRatio > fadeOutStartRatio)
            {
                // 縮小開始タイミングからの経過割合を計算し、1.0 から 0.0 へ減少させる
                float fadeProgress = (progressRatio - fadeOutStartRatio) / (1.0f - fadeOutStartRatio);
                // Mathf.Lerpで滑らかに1から0へ落とす
                fadeScaleMultiplier = Mathf.Lerp(1.0f, 0.0f, fadeProgress);

                rectTransform.localScale *= fadeScaleMultiplier;
            }
            else
            {
                // --- 2. シーソーのアニメーション（Z軸の回転） ---
                // 縮小に合わせて激しさも少し抑えるために、fadeScaleMultiplierを乗算（お好みで外してもOK）
                float currentMaxRotation = maxRotationAngle * fadeScaleMultiplier;
                float zRotation = Mathf.Sin(Time.time * rotationSpeed) * currentMaxRotation;
                rectTransform.localRotation = Quaternion.Euler(0f, 0f, zRotation);

                // --- 3. 拡縮のアニメーション（スケール） ---
                float scaleOffset = Mathf.Cos(Time.time * scaleSpeed) * scaleAmplitude;
                // 基本のシーソー拡縮を計算したあと、全体の消滅倍率（fadeScaleMultiplier）を掛ける
                Vector3 animatedScale = baseScale + new Vector3(scaleOffset, scaleOffset, 0f);
                rectTransform.localScale = animatedScale;
            }

            scoreText.color = GamingColor.GetRainbowColor(rainbowSpeed);

            time += Time.deltaTime;

            yield return null;
        }

        // 時間が来たらオブジェクトを削除
        Destroy(gameObject);
    }
}