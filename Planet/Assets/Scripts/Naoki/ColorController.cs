using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorController : MonoBehaviour
{
    [Header("カラー設定")]
    [SerializeField, Tooltip("適用したいベースカラー（RGB）")]
    private Color targetColor = Color.white;

    [Header("アルファ（透明度）制御")]
    [SerializeField, Tooltip("true: 下のAlpha値で上書きする / false: 他のスクリプトのAlpha値を維持する")]
    private bool overwriteAlpha = false;

    [SerializeField, Range(0f, 1f), Tooltip("overwriteAlphaがtrueのときのみ適用される透明度")]
    private float alphaValue = 1.0f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 他のスクリプト（RainbowSpriteなど）のUpdateの後に実行させるため、LateUpdateを使用します
    void LateUpdate()
    {
        if (spriteRenderer == null) return;

        // 新しいカラーのRGBを設定
        Color finalColor = targetColor;

        if (overwriteAlpha)
        {
            // アルファ値「有り」：インスペクターで指定した alphaValue を適用
            finalColor.a = alphaValue;
        }
        else
        {
            // アルファ値「無し」：現在のSpriteRendererのアルファ値をそのまま引き継ぐ（上書きしない）
            finalColor.a = spriteRenderer.color.a;
        }

        // 最終的な色を適用
        spriteRenderer.color = finalColor;
    }

    // 外部のスクリプトから動的に色を変えたい場合の関数（必要に応じて使ってください）
    public void SetColor(Color newColor, bool includeAlpha)
    {
        targetColor = newColor;
        overwriteAlpha = includeAlpha;
        if (includeAlpha)
        {
            alphaValue = newColor.a;
        }
    }
}