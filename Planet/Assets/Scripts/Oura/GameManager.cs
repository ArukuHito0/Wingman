using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

    void Start()
    {
        // 念のため最初は非表示
        gameOverUI.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        // ここで表示
        gameOverUI.SetActive(true);
        Debug.Log(gameOverUI.activeSelf);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}