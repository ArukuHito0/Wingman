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

        // durationが0以下のときにフリーズするのを防ぐ
        float clampedDuration = Mathf.Max(duration, 0.01f);

        // ゲーム実行中、またはエディタが動いている（画面が更新されている）間の時間を加算
        // エディタ上では Time.deltaTime が不安定になることがあるため、UnityEditorの更新に合わせます
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            // エディタ停止時は、前回のフレームからの経過時間を大まかに計算
            // （Sceneビューを動かしたり、インスペクターを触ったりすると進みます）
            hue += 0.02f / clampedDuration;
        }
        else
#endif
        {
            hue += Time.deltaTime / clampedDuration;
        }

        // hueが1を超えたら0に戻す
        if (hue > 1.0f)
        {
            hue -= 1.0f;
        }

        // HSVからRGBカラーに変換してスプライトに適用
        spriteRenderer.color = Color.HSVToRGB(hue, 1.0f, 1.0f);
    }
}