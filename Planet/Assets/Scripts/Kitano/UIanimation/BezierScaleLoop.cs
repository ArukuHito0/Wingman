using UnityEngine;

public class BezierScaleLoop : MonoBehaviour
{
    [Header("=== 移動（ベジェ）===")]
    public Transform startPoint;
    public Transform controlPoint;
    public Transform endPoint;

    public float moveDuration = 2f;
    public AnimationCurve moveEasing = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("=== 縮小 ===")]
    public Vector3 targetScale = Vector3.zero;
    public float shrinkSpeed = 1f;
    public float shrinkStartDelay = 0f;

    private float moveTimer = 0f;
    private float shrinkTimer = 0f;

    private Vector3 initialScale;
    private bool isShrinking = false;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        MoveAlongBezier();
        HandleScaling();
    }

    // =========================
    // ベジェ移動
    // =========================
    void MoveAlongBezier()
    {
        if (startPoint == null || controlPoint == null || endPoint == null) return;

        moveTimer += Time.deltaTime;

        float t = moveTimer / moveDuration;
        t = moveEasing.Evaluate(t);

        Vector3 pos =
            Mathf.Pow(1 - t, 2) * startPoint.position +
            2 * (1 - t) * t * controlPoint.position +
            Mathf.Pow(t, 2) * endPoint.position;

        transform.position = pos;

        // ループ（瞬間戻り）
        if (moveTimer >= moveDuration)
        {
            moveTimer = 0f;
            transform.position = startPoint.position;

            // 縮小もリセットしたい場合はここをON
            ResetScale();
        }
    }

    // =========================
    // 縮小処理
    // =========================
    void HandleScaling()
    {
        shrinkTimer += Time.deltaTime;

        if (!isShrinking && shrinkTimer >= shrinkStartDelay)
        {
            isShrinking = true;
        }

        if (isShrinking)
        {
            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                targetScale,
                shrinkSpeed * Time.deltaTime
            );
        }
    }

    // =========================
    // リセット処理
    // =========================
    void ResetScale()
    {
        transform.localScale = initialScale;
        shrinkTimer = 0f;
        isShrinking = false;
    }

    // =========================
    // デバッグ用：曲線表示
    // =========================
    void OnDrawGizmos()
    {
        if (startPoint == null || controlPoint == null || endPoint == null) return;

        Gizmos.color = Color.green;

        Vector3 prev = startPoint.position;

        for (int i = 1; i <= 20; i++)
        {
            float t = i / 20f;

            Vector3 pos =
                Mathf.Pow(1 - t, 2) * startPoint.position +
                2 * (1 - t) * t * controlPoint.position +
                Mathf.Pow(t, 2) * endPoint.position;

            Gizmos.DrawLine(prev, pos);
            prev = pos;
        }
    }
}