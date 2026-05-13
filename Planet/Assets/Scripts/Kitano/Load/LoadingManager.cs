using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;

    void Start()
    {
        Debug.Log("LoadingManager Start");

        NetworkManagerMock.Init(this);

        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        Debug.Log("nextScene = " + nextScene);

        // nextScene未設定なら戻す
        if (string.IsNullOrEmpty(nextScene))
        {
            Debug.LogWarning("nextScene 未設定");

            SceneManager.LoadScene(SceneNames.Puzzle);

            yield break;
        }

        // 通信開始
        NetworkManagerMock.ReceiveData();

        // 次シーンロード
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        op.allowSceneActivation = false;

        float timer = 0f;

        float minTime = 2f;

        while (true)
        {
            timer += Time.deltaTime;

            bool isLoadDone = (op.progress >= 0.9f);

            bool isNetworkDone = NetworkManagerMock.isReceived;

            bool isMinTime = (timer >= minTime);

            if (isLoadDone && isNetworkDone && isMinTime)
            {
                Debug.Log("ロード完了");

                op.allowSceneActivation = true;

                break;
            }

            yield return null;
        }
    }
}