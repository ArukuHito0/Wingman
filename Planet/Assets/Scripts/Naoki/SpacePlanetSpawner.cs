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
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private float maxSpawnInterval = 5.0f;

    // 表示用
    private float spawnInterval;
    [SerializeField, ReadOnly] private float currentSpawnInterval; // インスペクター表示用（グレーアウトする）

    //private float minSpeed = 1.0f;
    //private float maxSpeed = 5.0f;
    private float timer;

    private List<GameObject> spawndPlanets = new List<GameObject>();

    [SerializeField] private List<GameObject> planetPrefabList = new List<GameObject>();

    [SerializeField] private List<PlanetSpeedData> planetSpeedSettings = new List<PlanetSpeedData>();

    // public PlanetHistoryManager planetHistoryManager;
    private int planetHistoryLevel;
    private float planetHistoryTime;

    private int currentHistoryIndex = 0;
    [SerializeField] private bool autoSpawnMode = false;

    //フェーズ
    [SerializeField] private List<SpawnPhaseData> phaseList = new List<SpawnPhaseData>();
    private int currentPhaseIndex = 0;
    private float phaseTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        autoSpawnMode = true;
        phaseTimer = 0;

        if (PlanetHistoryManager.Instance == null || PlanetHistoryManager.Instance.history ==null)
        {
            autoSpawnMode = true;
            return;
        }

        //if (PlanetHistoryManager.Instance.history.Count == 0)
        //{
        //    Debug.Log("惑星の自動生成モードをON");
        //    autoSpawnMode = true;
        //}
        //else
        //{
        //    autoSpawnMode = false;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnInterval = spawnInterval;
        phaseTimer += Time.deltaTime;

        if (currentPhaseIndex + 1 < phaseList.Count)
        {
            if (phaseTimer >= phaseList[currentPhaseIndex + 1].startTime)
            {
                currentPhaseIndex++;
                Debug.Log($"フェーズが切り替わりました : {phaseList[currentPhaseIndex].phaseName}");
            }
        }

        if (autoSpawnMode == true)
        {
            // 自動生成のロジック
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                //最大数に達していない時だけ生成
                if (spawndPlanets.Count < maxObjectCount)
                {
                    AutoSpawnPlanet();
                }
            }
        }

        if (autoSpawnMode == false)
        {
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
        }
        CheckAndDespawn();  //削除のチェック
    }

    void AutoSpawnInterval()
    {
        timer = 0;
        spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void SpawnPlanet(int levelIndex)
    {
        // 防御策
        if (planetPrefabList.Count == 0)
        {
            Debug.LogWarning("惑星プレハブがリストに登録されていません！");
            Debug.Log("惑星の自動生成モードをON");
            autoSpawnMode = true;
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

        //// 向き * ランダムな速さ を速度としてセットする
        //float randomSpeed = Random.Range(minSpeed, maxSpeed);

        // 向き * 惑星ごとのランダムな速さ を速度としてセットする
        float finalSpeed = 1.0f;
        if (levelIndex < planetSpeedSettings.Count)
        {
            // 該当するレベルの速度データを取得
            PlanetSpeedData speedData = planetSpeedSettings[levelIndex];

            // データを使ってランダムな速度を計算する
            finalSpeed = Random.Range(speedData.minSpeed, speedData.maxSpeed);
        }
        float randomSpeed = finalSpeed;
        rb.linearVelocity = movementDir * randomSpeed;

        spawndPlanets.Add(newPlanet);
    }

    int GetRandomLevelIndexByWeight()
    {
        // 防御策 : フェーズデータがない場合は0を返す
        if (phaseList.Count == 0 || currentPhaseIndex >= phaseList.Count) return 0;

        // 現在のフェーズの確率リストを取得(Inspectorで設定)
        List<int> weights = phaseList[currentPhaseIndex].spawnWeights;

        // 1. 全てのウェイトの合計値を計算
        int totalWeight = 0;
        foreach (int w in weights)
        {
            totalWeight += w;
        }

        // ウェイトが設定されていなければ0を返す
        if (totalWeight <= 0) return 0;

        // 2. 0から 合計値(未満)の間のランダムな数字1つ決める
        int randomNumber = Random.Range(0, totalWeight);

        // 3. 当選確率のエリアを特定する(累積値のチェック)
        int currentSum = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            currentSum += weights[i];

            // ランダムな値が、現在の累積値を越えなければその要素(i)に決定する
            if (randomNumber < currentSum)
            {
                return i;
            }
        }
        return 0;
    }

    void AutoSpawnPlanet()
    {
        // 0番目から数えて「リストの数」未満のランダムな数字を得る
        // int autoLevelIndex = Random.Range(0, planetPrefabList.Count);
        int autoLevelIndex = GetRandomLevelIndexByWeight();

        GameObject autoSelectedPrefab = planetPrefabList[autoLevelIndex];

        Vector2 spawnPos = Random.insideUnitCircle.normalized * spawnRadius;
        Vector2 targetPos = Random.insideUnitCircle.normalized * targetRadius;
        Vector2 movementDir = (targetPos - spawnPos).normalized;

        // 取り出した[selectedPrefab]を生成する
        GameObject newPlanet = Instantiate(autoSelectedPrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = newPlanet.GetComponent<Rigidbody2D>();

        //// 向き * ランダムな速さ を速度としてセットする
        //float randomSpeed = Random.Range(minSpeed, maxSpeed);

        // 向き * 惑星ごとのランダムな速さ を速度としてセットする
        float finalSpeed = 1.0f;
        if (autoLevelIndex < planetSpeedSettings.Count)
        {
            // 該当するレベルの速度データを取得
            PlanetSpeedData speedData = planetSpeedSettings[autoLevelIndex];

            // データを使ってランダムな速度を計算する
            finalSpeed = Random.Range(speedData.minSpeed, speedData.maxSpeed);
        }

        float randomSpeed = finalSpeed;
        rb.linearVelocity = movementDir * randomSpeed;

        spawndPlanets.Add(newPlanet);
        AutoSpawnInterval();
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

    [System.Serializable]
    public struct PlanetSpeedData
    {
        public float minSpeed;
        public float maxSpeed;
    }

    [System.Serializable]
    public struct SpawnPhaseData
    {
        public string phaseName;        // Inspector用の名前
        public float startTime;         // このフェーズが始まる時間(s)

        // 要素数8のリスト
        // [100, 0, 0, 0, 0, 0, 0, 0]の場合 Element 0 が100%生成
        public List<int> spawnWeights;
    }
}