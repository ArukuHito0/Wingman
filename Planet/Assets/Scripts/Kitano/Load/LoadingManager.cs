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
        Debug.Log("Load 開始");

        Debug.Log("nextScene = " + nextScene);

        // 安全装置
        if (string.IsNullOrEmpty(nextScene))
        {
            Debug.LogError("nextScene が空です");
            yield break;
        }

        if (nextScene == "LoadingScene")
        {
            Debug.LogError("LoadingSceneをロードしようとしている");
            yield break;
        }

        Debug.Log("通信開始");

        // 通信開始
        NetworkManagerMock.ReceiveData();

        Debug.Log("シーンロード開始");

        // 非同期ロード
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

            Debug.Log(
                "LoadDone: " + isLoadDone +
                " / NetworkDone: " + isNetworkDone +
                " / MinTime: " + isMinTime
            );

            if (isLoadDone && isNetworkDone && isMinTime)
            {
                Debug.Log("ロード完了 → シーン移動");

                op.allowSceneActivation = true;

                break;
            }

            yield return null;
        }
    }
}