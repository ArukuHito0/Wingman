using UnityEngine;

public class UIAlphaTrigger : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeSpeed = 5f;
    private bool isPlayerInside = false;

    // プレイヤーが入ったとき
    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.CompareTag("Player") || other.CompareTag("Planet"))
        if (other.CompareTag("Player"))

        {
            isPlayerInside = true;
        }
    }

    // プレイヤーが中にいる間（念のためのバックアップ）
    void OnTriggerStay2D(Collider2D other)
    {
        //if (other.CompareTag("Player") || other.CompareTag("Planet"))
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    // プレイヤーが出ていったとき
    void OnTriggerExit2D(Collider2D other)
    {
        //if (other.CompareTag("Player") || other.CompareTag("Planet"))
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    void Update()
    {
        // 状態に応じてアルファ値を滑らかに変更
        float targetAlpha = isPlayerInside ? 0.1f : 1.0f;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }
}