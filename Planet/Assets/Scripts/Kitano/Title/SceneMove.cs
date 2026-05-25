using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public void GoToGameScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}