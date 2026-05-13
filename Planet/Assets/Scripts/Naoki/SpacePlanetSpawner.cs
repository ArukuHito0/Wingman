using NUnit.Framework;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class SpacePlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private float spawnRadius = 12f;
    [SerializeField] private float targetRadius = 3f;
    [SerializeField] private float despawnRadius = 15f;
    [SerializeField] private int maxObjectCount = 10;
    [SerializeField] private float spawnInterval = 2.0f;

    private float minSpeed = 1.0f;
    private float maxSpeed = 5.0f;
    private float timer;

    private List<GameObject> spawndPlanets = new List<GameObject>();

    [SerializeField] private List<GameObject> planetPrefabList = new List<GameObject>();

    // public PlanetHistoryManager planetHistoryManager;
    private int planetHistoryLevel;
    private float planetHistoryTime;

    private int currentHistoryIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 自動生成のロジック
        timer += Time.deltaTime;
        //if (timer >= spawnInterval)
        //{
        //    //最大数に達していない時だけ生成
        //    if (spawndPlanets.Count < maxObjectCount)
        //    {
        //        SpawnPlanet();
        //    }
        //    timer = 0;  // タイマーをリセット
        //}

        PlanetHistoryData nextData = PlanetHistoryManager.Instance.history[currentHistoryIndex];
        
        if (PlanetHistoryManager.Instance != null && currentHistoryIndex < PlanetHistoryManager.Instance.history.Count)
        {
            if (timer >= nextData.time)
            {
                int index = Mathf.Clamp(nextData.level, 0, planetPrefabList.Count - 1);

                SpawnPlanet(index);

                currentHistoryIndex++;
            }
        }
        CheckAndDespawn();  //削除のチェック
    }

    void SpawnPlanet(int levelIndex)
    {
        // 防御策
        if (planetPrefabList.Count == 0)
        {
            Debug.LogWarning("惑星プレハブがリストに登録されていません！");
            return;
        }

        //// 0番目から数えて「リストの数」未満のランダムな数字を得る
        //int index = Random.Range(0, planetPrefabList.Count);
        GameObject selectedPrefab = planetPrefabList[levelIndex];


        Vector2 spawnPos = Random.insideUnitCircle.normalized * spawnRadius;
        Vector2 targetPos = Random.insideUnitCircle.normalized * targetRadius;
        Vector2 movementDir = (targetPos - spawnPos).normalized;

        // 取り出した[selectedPrefab]を生成する
        GameObject newPlanet = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = newPlanet.GetComponent<Rigidbody2D>();

        // 向き * ランダムな速さ を速度としてセットする
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        rb.linearVelocity = movementDir * randomSpeed;

        spawndPlanets.Add(newPlanet);
    }

    void GetRandomPointInCircle(float radius)
    {
        Vector2 point = Random.insideUnitCircle * radius;
    }

    void ManagePlantMovement()
    {

    }

    void CheckAndDespawn()
    {
        for (int i = spawndPlanets.Count - 1; i >= 0; i--)
        {
            if (spawndPlanets[i] == null)
            {
                spawndPlanets.RemoveAt(i);
                continue;
            }

            float dist = Vector2.Distance(Vector2.zero, spawndPlanets[i].transform.position);
            if (dist > despawnRadius)
            {
                Destroy(spawndPlanets[i]);
                spawndPlanets.RemoveAt(i);
            }
        }
    }
    void OnDrawGizmos()
    {
        // 1. 出現圏を緑で描画
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);

        // 2. 目標圏を黄で描画
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Vector3.zero, targetRadius);

        // 3. 消去圏を赤で描画
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, despawnRadius);

        // 4. 各惑星の進行方向を青線で描画
        if (spawndPlanets != null)
        {
            Gizmos.color = Color.blue;
            foreach (GameObject planet in spawndPlanets)
            {
                if (planet != null)
                {
                    Rigidbody2D rb = planet.GetComponent<Rigidbody2D>();
                    // 現在地から、1秒後の予想地点まで線を引く
                    Gizmos.DrawLine(planet.transform.position, (Vector2)planet.transform.position + rb.linearVelocity);
                }
            }
        }
    }
}