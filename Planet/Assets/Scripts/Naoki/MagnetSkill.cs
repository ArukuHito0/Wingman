using System.Collections.Generic;
using UnityEngine;

public class MagnetSkill : MonoBehaviour
{
    [Header("ステータス設定")]
    [SerializeField] private bool isInvincible = false; // 無敵状態フラグ

    [Header("磁力設定")]
    [SerializeField] private float pullRadius = 5f;    // 引き寄せ半径
    [SerializeField] private float pullForce = 10f;    // 引き寄せの強さ
    [SerializeField] private List<string> targetTags = new List<string> { "Item", "Coin" }; // 対象のタグ（複数指定可）

    // 外部から無敵状態を切り替えるためのプロパティ（他のスクリプトから操作用）
    public bool IsInvincible
    {
        get { return isInvincible; }
        set { isInvincible = value; }
    }

    private void FixedUpdate()
    {
        // 無敵状態の時だけ引き寄せ処理を行う
        if (PlayerHealth.Instance != null && PlayerHealth.Instance.isStarInvincible == true)
        {
            PullObjects();
        }
    }

    private void PullObjects()
    {
        // プレイヤーを中心に、指定した半径内のコライダーをすべて取得
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pullRadius);

        foreach (Collider2D collider in colliders)
        {
            // 自身（プレイヤー）は除外
            if (collider.gameObject == this.gameObject) continue;

            // リストに登録されたタグの中に、検知したオブジェクトのタグがあるかチェック
            if (targetTags.Contains(collider.tag))
            {
                // リジッドボディの取得（これがないと動かせない）
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // プレイヤーへの方向ベクトルを計算
                    Vector2 direction = (transform.position - collider.transform.position).normalized;

                    // 徐々に加速して引き寄せる（力＝方向 × 強さ）
                    rb.AddForce(direction * pullForce);

                    // 【オプション】もし完全に吸い付くようにしたい（慣性を消したい）場合は
                    // 以下のコードに差し替えるか、オブジェクト側のLinear Drag（空気抵抗）を上げてください
                    // rb.linearVelocity = direction * pullForce; (Unity 2023以降) / rb.velocity = direction * pullForce; (それ以前)
                }
            }
        }
    }

    // エディタのSceneビュー上で引き寄せ範囲を円で視覚化する（デバッグ用）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}