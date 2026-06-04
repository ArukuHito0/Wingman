using UnityEngine;
using System.Collections;

public class MultiDirectionSlideOut : MonoBehaviour
{
    [Header("左へ飛ばすUI")]
    public RectTransform[] slideLeft;

    [Header("右へ飛ばすUI")]
    public RectTransform[] slideRight;

    [Header("上へ飛ばすUI")]
    public RectTransform[] slideUp;

    [Header("下へ飛ばすUI")]
    public RectTransform[] slideDown;

    [Header("移動時間")]
    public float duration = 0.5f;

    [Header("移動距離")]
    public float distance = 2000f;

    public void SlideOut()
    {
        foreach (RectTransform ui in slideLeft)
        {
            StartCoroutine(SlideCoroutine(ui, Vector2.left * distance));
        }

        foreach (RectTransform ui in slideRight)
        {
            StartCoroutine(SlideCoroutine(ui, Vector2.right * distance));
        }

        foreach (RectTransform ui in slideUp)
        {
            StartCoroutine(SlideCoroutine(ui, Vector2.up * distance));
        }

        foreach (RectTransform ui in slideDown)
        {
            StartCoroutine(SlideCoroutine(ui, Vector2.down * distance));
        }
    }

    IEnumerator SlideCoroutine(RectTransform ui, Vector2 moveOffset)
    {
        if (ui == null) yield break;

        Vector2 startPos = ui.anchoredPosition;
        Vector2 endPos = startPos + moveOffset;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            // EaseOut
            t = 1f - Mathf.Pow(1f - t, 3f);

            ui.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            yield return null;
        }

        ui.anchoredPosition = endPos;
    }
}