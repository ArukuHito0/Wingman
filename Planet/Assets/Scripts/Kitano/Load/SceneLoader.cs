using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GoToShootingPhase()
    {
        Debug.Log("GoToShootingPhase 開始");

        Debug.Log("攻撃データ送信");

        LoadingManager.nextScene = "Shooting Phase";

        Debug.Log("nextScene 設定: " + LoadingManager.nextScene);

        Debug.Log("LoadingSceneへ移動");

        SceneManager.LoadScene("LoadingScene");
    }
}