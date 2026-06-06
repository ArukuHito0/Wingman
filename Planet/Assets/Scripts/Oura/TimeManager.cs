using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using UnityEditor;
using System.Collections;


public class TimerManager : MonoBehaviour
{
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

    public float timeLimit = 120f;
    public float countDownStartTime = 10;

    public bool finished = false;

    public string transitionSceneName = string.Empty;

    public TextMeshProUGUI countDownText;

    [Header("虹色エフェクトの設定")]
    public float rainbowSpeed = 0.5f;

    private RectTransform countDownTextTransform;

    private void Awake()
    {
        countDownText.text = string.Empty;
        countDownTextTransform = countDownText.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (finished) return;

        timeLimit -= Time.deltaTime;

        if (timeLimit < countDownStartTime)
        {
            ScoreManager.Instance.scoreTitleText.enabled = false;
            ScoreManager.Instance.scoreText.enabled = false;

            countDownText.color = GamingColor.GetRainbowColor(rainbowSpeed);

            countDownText.text = timeLimit.ToString("F0");

            // カウントダウンのテキストのアニメーション
            if (countDownTextTransform == null) return;

            float zRotation = Mathf.Sin(Time.time * rotationSpeed) * maxRotationAngle;
            countDownTextTransform.localRotation = Quaternion.Euler(0f, 0f, zRotation);

            float scaleOffset = Mathf.Cos(Time.time * scaleSpeed) * scaleAmplitude;
            Vector3 animatedScale = baseScale + new Vector3(scaleOffset, scaleOffset, 0f);

            countDownTextTransform.localScale = animatedScale;
        }

        if (timeLimit <= 0)
        {
            timeLimit = 0;
            finished = true;

            AudioManager.instance.StopBGM();

            SceneManager.LoadScene(transitionSceneName);
        }
    }
}