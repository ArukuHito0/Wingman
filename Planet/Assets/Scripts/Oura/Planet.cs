using UnityEngine;

public class Planet : MonoBehaviour
{
    public int level;

    public GameObject[] evolvePrefabs;

    public GameObject explosionPrefab;

    bool isMerging = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isMerging) return;

        Planet other =
            collision.GetComponent<Planet>();

        if (other == null) return;

        if (other.isMerging) return;

        // 同じレベルだけ
        if (other.level != level) return;

        // 重複生成防止
        if (other.GetInstanceID() <
            gameObject.GetInstanceID())
            return;

        isMerging = true;
        other.isMerging = true;

        Vector2 pos =
            (transform.position +
            other.transform.position) / 2f;

        // 最終進化
        if (level >= evolvePrefabs.Length - 1)
        {
            if (explosionPrefab != null)
            {
                Instantiate(
                    explosionPrefab,
                    pos,
                    Quaternion.identity
                );
            }

            Destroy(other.gameObject);
            Destroy(gameObject);

            return;
        }

        // 次進化生成
        GameObject next = Instantiate(
            evolvePrefabs[level + 1],
            pos,
            Quaternion.identity
        );

        next.GetComponent<Planet>().level =
            level + 1;

        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}