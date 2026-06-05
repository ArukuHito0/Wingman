using UnityEngine;
using TMPro;
using System.Collections;

public class FinishUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI finishText;

    [Header("テスト")]
    [SerializeField] private bool playOnStart = true;

    [Header("演出設定")]
    [SerializeField] private float startScale = 15f;
    [SerializeField] private float overshootScale = 0.8f;
    [SerializeField] private float scaleDuration = 1.0f;
    [SerializeField] private float bounceDuration = 0.15f;
    [SerializeField] private float waitTime = 1.0f;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Start()
    {
        finishText.gameObject.SetActive(false);

        if (playOnStart)
        {
            ShowFinish();
        }
    }

    private void Update()
    {
        // テスト用
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopAllCoroutines();
            ShowFinish();
        }
    }

    public void ShowFinish()
    {
        StartCoroutine(FinishSequence());
    }

    IEnumerator FinishSequence()
    {
        finishText.gameObject.SetActive(true);

        Color color = finishText.color;
        color.a = 1f;
        finishText.color = color;

        RectTransform rect = finishText.rectTransform;

        // 超巨大サイズから開始
        rect.localScale = Vector3.one * startScale;

        // ----------------------
        // 巨大 → 0.8倍まで縮小
        // 最初ゆっくり、後半加速
        // ----------------------
        float time = 0f;

        while (time < scaleDuration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / scaleDuration);

            // 加速カーブ
            t = t * t * t;

            float scale = Mathf.Lerp(
                startScale,
                overshootScale,
                t);

            rect.localScale = Vector3.one * scale;

            yield return null;
        }

        // ----------------------
        // 0.8倍 → 1.0倍へ戻る
        // ----------------------
        time = 0f;

        while (time < bounceDuration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / bounceDuration);

            rect.localScale = Vector3.Lerp(
                Vector3.one * overshootScale,
                Vector3.one,
                t);

            yield return null;
        }

        rect.localScale = Vector3.one;

        // 少し停止
        yield return new WaitForSeconds(waitTime);

        // ----------------------
        // フェードアウト
        // ----------------------
        time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(
                1f,
                0f,
                time / fadeDuration);

            color.a = alpha;
            finishText.color = color;

            yield return null;
        }

        finishText.gameObject.SetActive(false);
    }
}