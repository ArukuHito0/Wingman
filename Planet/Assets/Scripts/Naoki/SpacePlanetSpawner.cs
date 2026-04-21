//using NUnit.Framework;
//using UnityEngine;
//using System.Collections.Generic;

//public class SpacePlanetSpawner : MonoBehaviour
//{
//    [SerializeField] private GameObject planetPrefab;
//    [SerializeField] private float spawnRadius = 10f;
//    [SerializeField] private float targetRadius = 3f;
//    [SerializeField] private float despawnRadius = 15f;
//    [SerializeField] private int maxObjectCount = 10;

//    private float minSpeed = 1.0f;
//    private float maxSpeed = 5.0f;

//    private List<GameObject> spawndPlanets = new List<GameObject>();



//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    void SpawnPlanet()
//    {
//        GameObject newPlanet = Instantiate(planetPrefab, spawnPos, Quaternion.indentity);
//        Rigidbody2D rb = newPlanet.GetComponent<Rigidbody2D>();

//        // 向き * ランダムな速さ を速度としてセットする
//        float randomSpeed = Random.Range(minSpeed, maxSpeed);
//        //##rb.linearVelocity = movementDir * randomSpeed;

//        spawndPlanets.Add(newPlanet);
//    }

//    void GetRandomPointInCircle(float radius)
//    {
//        Random.insideUnitCircle{}
//    }

//    void ManagePlantMovement()
//    {

//    }

//    void CheckAndDespawn()
//    {
//        for (int i = spawndPlanets.Count - 1; i >= 0; i--)
//        {
//            if (spawndPlanets[i] == null)
//            {
//                spawndPlanets.RemoveAt(i);
//                continue;
//            }

//            float dist = Vector2.Distance(Vector2.zero, spawndPlanets[i].transform.position);
//            if (dist > despawnRadius)
//            {
//                Destroy(spawndPlanets[i]);
//                spawndPlanets.RemoveAt(i);
//            }
//        }
//    }
//}
