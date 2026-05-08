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

        if (!p.isSettled) return;

        // 落として3秒以内は無視
        if (Time.time - p.dropTime < 3f) return;

        // 中心がライン超えてないなら無視
        if (p.transform.position.y < transform.position.y) return;

        gameOver = true;

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