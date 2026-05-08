using UnityEngine;

public class UIRectAlphaController : MonoBehaviour
{
    public RectTransform uiRect;    // 対象のUI（長方形）
    public Transform player;       // プレイヤー
    public CanvasGroup canvasGroup; // UIの透明度管理用
    public float fadeSpeed = 0.5f;    // 消えるスピード

    void Update()
    {
        // 安全装置：どれか一つでも空なら処理しない
        if (uiRect == null || player == null || canvasGroup == null || Camera.main == null)
        {
            return;
        }

        // プレイヤーの座標をスクリーン座標に変換
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(player.position);

        // 判定
        bool isOverlapping = RectTransformUtility.RectangleContainsScreenPoint(uiRect, playerScreenPos);

        // 反映
        canvasGroup.alpha = isOverlapping ? 0.3f : 1.0f;
    }
}