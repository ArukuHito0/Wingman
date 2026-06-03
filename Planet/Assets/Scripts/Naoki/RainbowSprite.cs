using UnityEngine;

// この属性を追加することで、ゲームを実行していなくてもエディタ上で動作します
[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class RainbowSprite : MonoBehaviour
{
    [SerializeField, Tooltip("虹色が一周するスピード（秒数）")]
    private float duration = 3.0f;

    private SpriteRenderer spriteRenderer;
    private float hue = 0.0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (spriteRenderer == null) return;

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

        // --- ここから修正 ---
        // 1. HSVからベースとなるRGBカラーを生成
        Color newColor = Color.HSVToRGB(hue, 1.0f, 1.0f);

        // 2. 他のスクリプトが変更したかもしれない「現在のアルファ値」を代入
        newColor.a = spriteRenderer.color.a;

        // 3. 反映
        spriteRenderer.color = newColor;
    }
}