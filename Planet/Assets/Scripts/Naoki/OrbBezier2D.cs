using System.Collections;
using UnityEngine;

public class OrbBezier2D : MonoBehaviour
{
    [SerializeField] private float gaugeValueAmount = 1.0f; // このオーブで増えるゲージ量
    [SerializeField] private float flyDuration = 1f;     // 飛んでいく時間
    [SerializeField] private float curveStrength = 100f;   // カーブの膨らみ具合

    private bool isCollected = false;
    private Collider2D myCollider;
    private SpriteRenderer myRenderer;

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 通常通りプレイヤーが直接触れた場合（単体で拾った時用）
        if (other.CompareTag("Player") && !isCollected)
        {
            // 単体の時は、自分で予測ゲージを伸ばして、その場所を取得して飛ぶ
            GaugeManager.Instance.PredictGainGauge(gaugeValueAmount);
            Vector3 targetPos = GaugeManager.Instance.GetTargetWorldPosition();
            CollectAndFly(targetPos);
        }
    }

    // ★修正：外部から「共通の目的地」を受け取って飛ぶためのメソッド
    public void CollectAndFly(Vector3 sharedTargetPosition)
    {
        if (isCollected) return;

        isCollected = true;
        if (myCollider != null) myCollider.enabled = false; // 二重衝突防止

        // 指定された目的地へ向けてルーチンを開始
        StartCoroutine(FlyRoutine(sharedTargetPosition));
    }

    // ★引数で目的地を受け取るように変更
    private IEnumerator FlyRoutine(Vector3 targetPos)
    {
        // 1. 開始位置の記録
        Vector3 startPos = transform.position;

        // 2. 【上まわりカーブの制御点を計算】
        Vector3 middlePos = Vector3.Lerp(startPos, targetPos, 0.5f);
        Vector2 direction = (targetPos - startPos).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        // 目的地が左側なら垂直ベクトルを反転して常に上凸にする
        if (direction.x < 0f)
        {
            perpendicular = -perpendicular;
        }
        Vector3 controlPos = middlePos + (Vector3)perpendicular * curveStrength;

        // 3. ベジェ曲線アニメーションループ
        float elapsedTime = 0f;
        while (elapsedTime < flyDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / flyDuration;

            // 後半に向けてググッと加速（EaseInQuad）
            t = t * t;

            // ベジェ曲線補間
            Vector3 m1 = Vector3.Lerp(startPos, controlPos, t);
            Vector3 m2 = Vector3.Lerp(controlPos, targetPos, t);
            transform.position = Vector3.Lerp(m1, m2, t);

            // 飛びながらだんだん小さく、少し透明にする
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            if (myRenderer != null)
            {
                Color c = myRenderer.color;
                c.a = Mathf.Lerp(1f, 0.2f, t);
                myRenderer.color = c;
            }

            yield return null;
        }

        // 4. 【到着】実際のゲージの数値を増やし、前面ゲージをそこまで追いつかせる
        GaugeManager.Instance.GainGauge(gaugeValueAmount);

        // オブジェクトを破棄
        Destroy(gameObject);
    }
}