using UnityEngine;

public class Planet : MonoBehaviour
{
    public int level;

    public bool isSettled = false;

    private Rigidbody2D rb;

    public float dropTime;

    bool notified = false;

    public bool isDropped = false;

    private bool isMerging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 落下後止まったら積み上がった扱い
        if (!isSettled && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            if (rb.linearVelocity.magnitude < 0.05f)
            {
                isSettled = true;
            }
        }
    }

    // 着地通知だけ
    void OnCollisionEnter2D(
    Collision2D collision
)
    {
        if (notified) return;

        // 落とした惑星だけ
        if (!isDropped) return;

        notified = true;

        PlanetSpawner spawner =
            FindObjectOfType<PlanetSpawner>();

        spawner.OnPlanetLanded();
    }

    // 合体判定
    void OnCollisionStay2D(Collision2D collision)
    {
        if (isMerging) return;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.bodyType != RigidbodyType2D.Dynamic) return;

        Planet other = collision.gameObject.GetComponent<Planet>();
        if (other == null) return;

        if (other.isMerging) return;

        Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
        if (otherRb.bodyType != RigidbodyType2D.Dynamic) return;

        if (other.level != level) return;

 

        CircleCollider2D myCol =
           GetComponent<CircleCollider2D>();

        CircleCollider2D otherCol =
            other.GetComponent<CircleCollider2D>();

        float range =
            myCol.radius * transform.localScale.x +
            otherCol.radius * other.transform.localScale.x;

        float dist = Vector2.Distance(
            transform.position,
            other.transform.position
        );

        // 少し余裕を持たせる
        if (dist > range * 1.1f) return;

        if (other.GetInstanceID() < gameObject.GetInstanceID()) return;

        PlanetSpawner spawner = FindObjectOfType<PlanetSpawner>();



        // 合体開始
        isMerging = true;
        other.isMerging = true;

        // 最大レベルなら消える
        if (level >= spawner.smallPlanets.Length - 1)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }

        Vector2 pos =
            (transform.position + other.transform.position) / 2f;

        GameObject next = Instantiate(
            spawner.smallPlanets[level + 1],
            pos,
            Quaternion.identity
        );

        next.GetComponent<Planet>().level = level + 1;

        PlanetCounter.Instance.Add(level + 1);

        PlanetHistoryManager.Instance.AddHistory(level + 1);

        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}