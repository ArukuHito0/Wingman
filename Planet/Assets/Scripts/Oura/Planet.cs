using UnityEngine;

public class Planet : MonoBehaviour
{
    public int level;

    private bool isMerging = false; 

    void OnCollisionEnter2D(Collision2D collision)
    {
        //  すでに合体中なら無視
        if (isMerging) return;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.bodyType != RigidbodyType2D.Dynamic) return;

        Planet other = collision.gameObject.GetComponent<Planet>();
        if (other == null) return;

        //  相手も合体中なら無視
        if (other.isMerging) return;

        Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
        if (otherRb.bodyType != RigidbodyType2D.Dynamic) return;

        if (other.level != level) return;

        if (other.GetInstanceID() < gameObject.GetInstanceID()) return;

        PlanetSpawner spawner = FindObjectOfType<PlanetSpawner>();

        // 合体フラグON
        isMerging = true;
        other.isMerging = true;

        // 最大レベルなら消える
        if (level >= spawner.smallPlanets.Length - 1)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }

        Vector2 pos = (transform.position + other.transform.position) / 2f;

        GameObject next = Instantiate(
            spawner.smallPlanets[level + 1],
            pos,
            Quaternion.identity
        );

        next.GetComponent<Planet>().level = level + 1;
        PlanetCounter.Instance.Add(level + 1);
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}