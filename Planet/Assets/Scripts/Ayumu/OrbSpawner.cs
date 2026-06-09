using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [Header("オーブ生成の設定")]
    [SerializeField, Header("オーブのプレハブ")]
    private GameObject orbPrefab; // オーブのプレハブをインスペクターでセット

    [SerializeField, Header("スポーンするオーブ数")]
    private int spawnCount = 5;             // 生成するオーブの数

    [SerializeField, Header("オーブの飛散する半径")]
    private float scatterRadius = 1.0f;     // 飛び散る半径

    [SerializeField, Header("ゲージ上昇量")]
    private float gaugeAmount = 10f;

    public void SpawnOrb()
    {
        if (orbPrefab != null)
        {
            // ★ポイント1：ループの前に、3個分の合計ゲージ量をまとめて「先に」予測ゲージに反映させる
            GaugeManager.Instance.PredictGainGauge(gaugeAmount);

            // ★ポイント2：3個分伸びきった「一番最後の目的地」のワールド座標を1回だけ取得する
            Vector3 finalTargetPos = GaugeManager.Instance.GetTargetWorldPosition();

            // オーブの生成ループ
            for (int i = 0; i < spawnCount; i++)
            {
                // 惑星の周りに散らす位置計算
                float angle = (360f / spawnCount) * i * Mathf.Deg2Rad;
                Vector3 spawnOffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * scatterRadius;
                Vector3 spawnPosition = transform.position + spawnOffset;

                // オーブを生成
                GameObject spawnedOrb = Instantiate(orbPrefab, spawnPosition, Quaternion.identity);

                // 生成したオーブのスクリプトを取得
                OrbBezier2D orbScript = spawnedOrb.GetComponent<OrbBezier2D>();
                if (orbScript != null)
                {
                    // ★ポイント3：全員に共通の「最後の目的地」を渡して一斉に飛ばす
                    orbScript.CollectAndFly(finalTargetPos);
                }
            }
        }
    }
}
