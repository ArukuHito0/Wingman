using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;

    public Slider slider;
    public Text text;

    float current = 0f;

    void Start()
    {
        NetworkManagerMock.Init(this);
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        // 通信開始
        NetworkManagerMock.ReceiveData();

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        float minTime = 2f;

        while (true)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            current = Mathf.Lerp(current, progress, Time.deltaTime * 5f);

            slider.value = current;
            text.text = (current * 100f).ToString("F0") + "%";

            timer += Time.deltaTime;

            bool isLoadDone = (progress >= 1f);
            bool isNetworkDone = NetworkManagerMock.isReceived;
            bool isMinTime = (timer >= minTime);

            // ★ここが今回のキモ
            if (isLoadDone && isNetworkDone && isMinTime)
            {
                op.allowSceneActivation = true;
                break;
            }

            yield return null;
        }
    }
}