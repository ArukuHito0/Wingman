using UnityEngine;

public enum PlanetAttribute
{
    Normal,
    Rare,
    Explosion,
}

public class Planet : MonoBehaviour
{
    public int level;
    public PlanetAttribute attribute;

    // 次進化先
    public GameObject nextPlanetPrefab;

    private bool isMerging = false;

    public GameObject explosionPrefab;

    [SerializeField]
    private GameObject evolusionEffect;

    [SerializeField]
    private GameObject highlightEffect;

    [SerializeField]
    private GameObject explosionAuraEffect;

    private bool isInGravityHole = false;

    [Header("進化時のオーブ生成の設定")]
    [SerializeField, Header("オーブのプレハブ")]
    private GameObject orbPrefab; // オーブのプレハブをインスペクターでセット

    [SerializeField, Header("スポーンするオーブ数")]
    private int spawnCount = 5;             // 生成するオーブの数

    [SerializeField, Header("オーブの飛散する半径")]
    private float scatterRadius = 1.0f;     // 飛び散る半径

    [SerializeField, Header("進化時のゲージ上昇量")]
    private float gaugeAmount = 10f;

    /// <summary>
    /// 惑星属性セット
    /// </summary>
    public void SetAttribute(PlanetAttribute attribute)
    {
        this.attribute = attribute;

        GameObject effect = null;

        switch (this.attribute)
        {
            case PlanetAttribute.Rare:
                effect = highlightEffect;
                break;

            case PlanetAttribute.Explosion:
                effect = explosionAuraEffect;
                break;
        }

        if (effect != null)
        {
            Instantiate(
                effect,
                transform.position,
                Quaternion.identity,
                transform
            );
        }
    }

    /// <summary>
    /// 他の惑星と合体
    /// </summary>
    public void MergeWith(Planet other)
    {
        if (isMerging) return;

        if (other == null) return;

        if (other.isMerging) return;

        if (other.level != level) return;

        AudioManager.instance.PlaySE("Evo");

        isMerging = true;
        other.isMerging = true;

        // コライダー停止
        Collider2D myCol =
            GetComponent<Collider2D>();

        Collider2D otherCol =
            other.GetComponent<Collider2D>();

        if (myCol != null)
            myCol.enabled = false;

        if (otherCol != null)
            otherCol.enabled = false;

        // 合体位置
        Vector2 mergePos =
            (transform.position +
            other.transform.position) / 2f;

        // 最終進化
        if (nextPlanetPrefab == null)
        {
            if (explosionPrefab != null)
            {
                AudioManager.instance.PlaySE("BigBang");

                Instantiate(
                    explosionPrefab,
                    mergePos,
                    Quaternion.identity
                );
            }

            Destroy(other.gameObject);
            Destroy(gameObject);

            return;
        }

        // 次惑星生成
        GameObject nextPlanet =
            Instantiate(
                nextPlanetPrefab,
                mergePos,
                Quaternion.identity
            );

        Planet nextPlanetScript =
            nextPlanet.GetComponent<Planet>();

        if (nextPlanetScript != null)
        {
            nextPlanetScript.level =
                level + 1;

            var rnd = Random.value;
            PlanetAttribute attribute = PlanetAttribute.Normal;

            if (rnd < 0.7f)
                attribute = PlanetAttribute.Rare;
            else
                attribute = PlanetAttribute.Explosion;

            nextPlanetScript.SetAttribute(attribute);
        }

        // 速度引継ぎ
        Rigidbody2D myRb =
            GetComponent<Rigidbody2D>();

        Rigidbody2D otherRb =
            other.GetComponent<Rigidbody2D>();

        Rigidbody2D nextRb =
            nextPlanet.GetComponent<Rigidbody2D>();

        if (myRb != null &&
            otherRb != null &&
            nextRb != null)
        {
            Vector2 mergedVelocity =
                (myRb.linearVelocity +
                otherRb.linearVelocity) / 2f;

            nextRb.linearVelocity =
                mergedVelocity;
        }

        // エフェクト
        if (evolusionEffect != null)
        {
            Instantiate(
                evolusionEffect,
                mergePos,
                Quaternion.identity
            );
        }

        if (isInGravityHole == true)
        {
            Debug.Log("スキルで惑星進化");

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
            else
            {
                Debug.LogError("Planetスクリプトのインスペクターに orbPrefab がセットされていません！");
            }
        }

        Destroy(other.gameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// 単体進化
    /// </summary>
    public void Evolve()
    {
        if (isMerging) return;

        AudioManager.instance.PlaySE("Evo");

        isMerging = true;

        if (nextPlanetPrefab == null)
        {
            if (explosionPrefab != null)
            {
                AudioManager.instance.PlaySE("BigBang");

                Instantiate(
                    explosionPrefab,
                    transform.position,
                    Quaternion.identity
                );
            }

            Destroy(gameObject);

            return;
        }

        GameObject nextPlanet =
            Instantiate(
                nextPlanetPrefab,
                transform.position,
                Quaternion.identity
            );

        Planet nextPlanetScript =
            nextPlanet.GetComponent<Planet>();

        if (nextPlanetScript != null)
        {
            nextPlanetScript.level =
                level + 1;

            nextPlanetScript.SetAttribute(attribute);
        }

        Rigidbody2D myRb =
            GetComponent<Rigidbody2D>();

        Rigidbody2D nextRb =
            nextPlanet.GetComponent<Rigidbody2D>();

        if (myRb != null &&
            nextRb != null)
        {
            nextRb.linearVelocity =
                myRb.linearVelocity;
        }

        if (evolusionEffect != null)
        {
            Instantiate(
                evolusionEffect,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// トリガー判定（GravityHoleの出入りを監視）
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GravityHole"))
        {
            isInGravityHole = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GravityHole"))
        {
            isInGravityHole = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            Planet other =
            collision.gameObject
            .GetComponent<Planet>();

        if (other == null) return;

        MergeWith(other);
    }
}