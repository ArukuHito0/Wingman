using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetSpawner : MonoBehaviour
{
    public GameObject[] smallPlanets;

    public float spawnY = 4f;
    public float minX = -2.5f;
    public float maxX = 2.5f;

    public Transform nextDisplayPoint; // ネクスト表示位置

    private GameObject currentPlanet;
    private GameObject nextPreview;

    private int nextIndex;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        // 最初のネクストを決める
        nextIndex = Random.Range(0, 3);

        SpawnNewPlanet();
        ShowNext();
    }

    void Update()
    {
        MovePlanet();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            DropPlanet();
        }
    }

    void MovePlanet()
    {
        if (currentPlanet == null) return;

        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 world = cam.ScreenToWorldPoint(mousePos);
        world.z = 0f;

        // 半径取得（CircleCollider用）
        CircleCollider2D col = currentPlanet.GetComponent<CircleCollider2D>();
        float radius = col.radius * currentPlanet.transform.localScale.x;

        float x = Mathf.Clamp(world.x, minX + radius, maxX - radius);

        currentPlanet.transform.position = new Vector2(x, spawnY);
    }

    void DropPlanet()
    {
        if (currentPlanet == null) return;

        Rigidbody2D rb = currentPlanet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }

        //  当たり判定ON
        Collider2D col = currentPlanet.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
        }

        currentPlanet = null;

        Invoke(nameof(SpawnNewPlanet), 0.5f);
    }

    void SpawnNewPlanet()
    {
        int rand = nextIndex; // ネクスト使用

        // 次のネクストを決める
        nextIndex = Random.Range(0, 3);

        float x = Random.Range(minX, maxX);

        currentPlanet = Instantiate(
            smallPlanets[rand],
            new Vector2(x, spawnY),
            Quaternion.identity
        );

        // レベル設定
        currentPlanet.GetComponent<Planet>().level = rand;

        // 動かないように
        Rigidbody2D rb = currentPlanet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        //  当たり判定OFF
        Collider2D col = currentPlanet.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        ShowNext(); // ネクスト更新
    }

    void ShowNext()
    {
        // 前の表示削除
        if (nextPreview != null)
        {
            Destroy(nextPreview);
        }

        // 新しく生成
        nextPreview = Instantiate(
            smallPlanets[nextIndex],
            nextDisplayPoint.position,
            Quaternion.identity
        );

        // 見た目用（物理無効）
        Rigidbody2D rb = nextPreview.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Collider2D col = nextPreview.GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        // 少し小さく表示
        nextPreview.transform.localScale *= 0.7f;
    }
}