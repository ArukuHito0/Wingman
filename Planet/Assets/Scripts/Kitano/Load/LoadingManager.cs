using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static string nextScene;

    public Slider slider;
    public Text text;

    IEnumerator Start()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        float minTime = 2f;
        float current = 0f;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            current = Mathf.Lerp(current, progress, Time.deltaTime * 5f);

            slider.value = current;
            text.text = (current * 100f).ToString("F0") + "%";

            timer += Time.deltaTime;

            if (progress >= 1f && timer >= minTime)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}