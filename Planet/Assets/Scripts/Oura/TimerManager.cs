using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float timeLimit = 120f;

    public TextMeshProUGUI timerText;

    bool finished = false;

    void Update()
    {
        if (finished) return;

        timeLimit -= Time.deltaTime;

        if (timeLimit < 0)
        {
            timeLimit = 0;
        }

        int minute = Mathf.FloorToInt(timeLimit / 60);
        int second = Mathf.FloorToInt(timeLimit % 60);

        timerText.text =
            minute.ToString("00") +
            ":" +
            second.ToString("00");

        if (timeLimit <= 0)
        {
            finished = true;

            SceneManager.LoadScene("Shooting Phase");
        }
    }
}