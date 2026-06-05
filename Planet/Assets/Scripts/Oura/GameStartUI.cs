using UnityEngine;
using TMPro;
using System.Collections;

public class GameStartUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI dotText;
    [SerializeField] private TextMeshProUGUI startText;

    [Header("時間設定")]
    [SerializeField] private float dotInterval = 0.4f;
    [SerializeField] private float startDisplayTime = 0.5f;

    private void Start()
    {
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        // 初期設定
        missionText.gameObject.SetActive(true);
        readyText.gameObject.SetActive(true);
        dotText.gameObject.SetActive(true);
        startText.gameObject.SetActive(false);

        dotText.text = "";

        SetAlpha(missionText, 1f);
        SetAlpha(readyText, 1f);
        SetAlpha(dotText, 1f);
        SetAlpha(startText, 1f);

        // READY表示
        yield return new WaitForSeconds(0.5f);

        // .
        dotText.text = ".";
        yield return new WaitForSeconds(dotInterval);

        // ..
        dotText.text = "..";
        yield return new WaitForSeconds(dotInterval);

        // ...
        dotText.text = "...";
        yield return new WaitForSeconds(dotInterval);

        // ミッション、READY、ドットを同時フェードアウト
        StartCoroutine(FadeOut(missionText, 0.5f));
        StartCoroutine(FadeOut(readyText, 0.5f));
        yield return StartCoroutine(FadeOut(dotText, 0.5f));

        missionText.gameObject.SetActive(false);
        readyText.gameObject.SetActive(false);
        dotText.gameObject.SetActive(false);

        // START表示
        startText.gameObject.SetActive(true);
        startText.rectTransform.localScale = Vector3.one;

        // STARTを弾ませる
        yield return StartCoroutine(BounceText(startText));

        // 少し表示
        yield return new WaitForSeconds(startDisplayTime);

        // STARTフェードアウト
        yield return StartCoroutine(FadeOut(startText, 0.5f));

        startText.gameObject.SetActive(false);
    }

    IEnumerator BounceText(TextMeshProUGUI text)
    {
        RectTransform rect = text.rectTransform;

        Vector3 normal = Vector3.one;
        Vector3 big = Vector3.one * 1.5f;
        Vector3 small = Vector3.one * 0.9f;

        float duration = 0.15f;
        float time = 0f;

        // 拡大
        while (time < duration)
        {
            time += Time.deltaTime;
            rect.localScale = Vector3.Lerp(normal, big, time / duration);
            yield return null;
        }

        // 縮小
        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            rect.localScale = Vector3.Lerp(big, small, time / duration);
            yield return null;
        }

        // 元サイズへ
        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            rect.localScale = Vector3.Lerp(small, normal, time / duration);
            yield return null;
        }

        rect.localScale = normal;
    }

    IEnumerator FadeOut(TextMeshProUGUI text, float duration)
    {
        Color color = text.color;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, time / duration);
            text.color = color;

            yield return null;
        }

        color.a = 0f;
        text.color = color;
    }

    void SetAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}