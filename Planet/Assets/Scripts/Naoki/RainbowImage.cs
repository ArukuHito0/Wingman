using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]

public class RainbowImage : MonoBehaviour
{
    [SerializeField, Tooltip("虹色が一周するスピード（秒数）")]
    private float duration = 3.0f;

    private Image uiImage;
    private float hue = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (uiImage == null) return;

        float clampedDuration = Mathf.Max(duration, 0.01f);

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            hue += 0.02f / clampedDuration;
        }
        else
#endif
        {
            hue += Time.deltaTime / clampedDuration;
        }

        if (hue > 1.0f)
        {
            hue -= 1.0f;
        }

        // --- 修正箇所はここから ---

        // 1. まず、現在のUI画像が持っているアルファ値（透明度）を保存しておく
        float currentAlpha = uiImage.color.a;

        // 2. 虹色（RGB）を計算する
        Color rainbowColor = Color.HSVToRGB(hue, 1.0f, 1.0f);

        // 3. 虹色のRGBに、保存しておいたアルファ値を合体させる
        rainbowColor.a = currentAlpha;

        // 4. 最終的な色をUIに適用する
        uiImage.color = rainbowColor;
    }
}
