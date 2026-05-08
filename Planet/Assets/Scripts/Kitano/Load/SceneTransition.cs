using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTransition
{
    public static void Load(string nextScene)
    {
        Debug.Log("シーン遷移開始 : " + nextScene);

        // 次シーン設定
        LoadingManager.nextScene = nextScene;

        // ロード画面へ
        SceneManager.LoadScene("LoadingScene");
    }
}