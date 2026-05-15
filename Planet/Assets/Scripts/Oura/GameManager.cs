using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Transform blackHoleCenter;
    public BlackHoleController hole;

    public PlanetHistoryAPI historyAPI;
    public void StartGameOver()
    {
        PlanetSpawner spawner = FindObjectOfType<PlanetSpawner>();
        spawner.isGameOver = true;

        StartCoroutine(GameOverRoutine());

        // 履歴保存
        historyAPI.SendHistory();

    }

    IEnumerator GameOverRoutine()
    {
        PlanetSpawner spawner = FindObjectOfType<PlanetSpawner>();

        if (spawner.currentPlanet != null)
        {
            Destroy(spawner.currentPlanet);
            spawner.currentPlanet = null;
        }

        Planet[] planets = FindObjectsOfType<Planet>();

        float timer = 0f;


        while (timer < 2f)
        {
            foreach (Planet p in planets)
            {
                if (p == null) continue;

                Vector3 dir = (blackHoleCenter.position - p.transform.position).normalized;

                p.transform.position += dir * 5f * Time.deltaTime;

                p.transform.localScale *= 0.98f;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // 全消し
        foreach (Planet p in FindObjectsOfType<Planet>())
        {
            Destroy(p.gameObject);
        }

        // 再スタート
        FindObjectOfType<PlanetSpawner>().ResetGame();
    }
}
