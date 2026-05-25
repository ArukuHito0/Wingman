using UnityEngine;

public class Planet : MonoBehaviour
{
    public int level;

    // 次進化先
    public GameObject nextPlanetPrefab;

    private bool isMerging = false;

    public GameObject explosionPrefab;

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

        // 吹っ飛び
        Rigidbody2D rb =
            nextPlanet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            Vector2 dir =
                Random.insideUnitCircle.normalized;

            float speed =
                Random.Range(1f, 3f);

            rb.linearVelocity =
                dir * speed;

            rb.angularVelocity =
                Random.Range(-200f, 200f);
        }

        // エフェクト
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
    }
}