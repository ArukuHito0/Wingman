using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    [Header("ターゲットの設定")]
    [Tooltip("引き寄せ対象のタグリスト")]
    public List<string> targetTags = new List<string> { "Item", "Enemy" };

    [Header("引き寄せのパラメータ")]
    [Tooltip("移動速度")]
    public float speed = 5f;

    [Tooltip("この距離まで近づいたら追従を優先する（0なら常に最寄り）")]
    public float detectionRadius = 10f;

    private Transform currentTarget;

    void Update()
    {
        // ターゲットを探す（毎フレーム探すと重いため、見失った時や一定周期が理想ですが、シンプル化のためUpdateに配置）
        FindClosestTarget();

        // ターゲットが存在すれば引き寄せ処理を行う
        if (currentTarget != null)
        {
            MoveTowardsTarget();
        }
    }

    /// <summary>
    /// 登録されたタグを持つオブジェクトの中から、最も近いものを探す
    /// </summary>
    void FindClosestTarget()
    {
        float closestDistance = Mathf.Infinity;
        Transform closestTransform = null;
        Vector3 currentPosition = transform.position;

        // 指定されたすべてのタグをループ
        foreach (string tag in targetTags)
        {
            if (string.IsNullOrEmpty(tag)) continue;

            try
            {
                // そのタグを持つオブジェクトをすべて取得
                GameObject[] targetsWithTag = GameObject.FindGameObjectsWithTag(tag);

                foreach (GameObject targetObj in targetsWithTag)
                {
                    float distance = Vector3.Distance(targetObj.transform.position, currentPosition);

                    // 検出範囲内、かつこれまでの最寄りより近ければキープ
                    if (distance < closestDistance && distance <= detectionRadius)
                    {
                        closestDistance = distance;
                        closestTransform = targetObj.transform;
                    }
                }
            }
            catch (System.Exception)
            {
                Debug.LogWarning($"タグ「{tag}」はUnityのエディタ側で定義されていない可能性があります。");
            }
        }

        currentTarget = closestTransform;
    }

    /// <summary>
    /// ターゲットに向かって移動する
    /// </summary>
    public void MoveTowardsTarget()
    {
        // 目標方向への移動ベクトルを計算
        Vector3 targetPosition = currentTarget.position;

        // 2Dの場合はZ軸を固定、3Dで高さを合わせたくない場合は Y軸を固定してください
        // targetPosition.y = transform.position.y; 

        // なめらかに移動
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 必要に応じてターゲットの方を向かせる場合（3D用）
        /*
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
        */
    }

    // 範囲を可視化するためのインスペクターギズモ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}