using UnityEngine;
using System.Collections;

public class GameOverLine : MonoBehaviour
{
    public bool gameOver = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (gameOver) return;

        Planet p = other.GetComponent<Planet>();
        if (p == null) return;

      
        // 中心がライン超えてないなら無視
        if (p.transform.position.y < transform.position.y) return;

        gameOver = true;

        Planet[] planets =
    FindObjectsOfType<Planet>();

        foreach (Planet planet in planets)
        {
            planet.enabled = false;

            Rigidbody2D rb =
                planet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.simulated = false;
            }
        }

        PlanetSpawner spawner = FindObjectOfType<PlanetSpawner>();
        spawner.isGameOver = true;

        StartCoroutine(GameOverDelay());
    }

    IEnumerator GameOverDelay()
    {
        // 2秒待つ
        yield return new WaitForSeconds(2f);

        FindObjectOfType<GameManager>().StartGameOver();
    }

    public void ResetLine()
    {
        gameOver = false;
    }
}