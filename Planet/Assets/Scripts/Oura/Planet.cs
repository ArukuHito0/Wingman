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
    [SerializeField] private GameObject evolusionEffect;
    [SerializeField] private GameObject highlightEffect;
    [SerializeField] private GameObject explosionAuraEffect;

    /// <summary>
    /// 惑星のレア度をセット
    /// </summary>
    public void SetAttribute(PlanetAttribute attribute)
    {
        this.attribute = attribute;

        GameObject effect = null;

        switch (this.attribute)
        {
            case PlanetAttribute.Rare:
                effect = highlightEffect; break;
            case PlanetAttribute.Explosion:
                effect = explosionAuraEffect; break;
            default:
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMerging) return;

        Planet other =
            collision.gameObject.GetComponent<Planet>();

        if (other == null) return;

        if (other.isMerging) return;

        // 同じレベルだけ
        if (other.level != level) return;

        // 二重生成防止
        if (other.GetInstanceID() <
            gameObject.GetInstanceID())
            return;

        isMerging = true;
        other.isMerging = true;

        // 合体位置
        Vector2 mergePos =
            (transform.position +
            other.transform.position) / 2f;

        // 次惑星が無いなら爆発だけ
        if (nextPlanetPrefab == null)
        {
            if (explosionPrefab != null)
            {
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

        // 次レベル設定
        Planet nextPlanetScript =
            nextPlanet.GetComponent<Planet>();

        if (nextPlanetScript != null)
        {
            nextPlanetScript.level =
                level + 1;
        }

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
            // 2つの速度を平均
            Vector2 mergedVelocity =
                (myRb.linearVelocity +
                 otherRb.linearVelocity);

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

        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}