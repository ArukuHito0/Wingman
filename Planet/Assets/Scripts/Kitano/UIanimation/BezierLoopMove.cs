using UnityEngine;

public class BezierLoopMove : MonoBehaviour
{
    [Header("Points")]
    public Transform startPoint;   // 開始地点
    public Transform controlPoint; // 制御点
    public Transform endPoint;     // 終了地点

    [Header("Motion")]
    public float duration = 2f; // 移動時間
    public AnimationCurve easing = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float timer = 0f;

    void Update()
    {
        if (startPoint == null || controlPoint == null || endPoint == null) return;

        // 時間更新
        timer += Time.deltaTime;

        float t = timer / duration;

        // イージング適用
        t = easing.Evaluate(t);

        // ベジェ曲線計算（二次）
        Vector3 pos =
            Mathf.Pow(1 - t, 2) * startPoint.position +
            2 * (1 - t) * t * controlPoint.position +
            Mathf.Pow(t, 2) * endPoint.position;

        transform.position = pos;

        // 終了したらループ（瞬間戻り）
        if (timer >= duration)
        {
            timer = 0f;
            transform.position = startPoint.position;
        }
    }
}