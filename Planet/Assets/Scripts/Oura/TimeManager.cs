using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using UnityEditor;


public class TimerManager : MonoBehaviour
{
    public float timeLimit = 120f;

    public bool finished = false;


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

        if (timeLimit <= 0)
        {
            finished = true;
        }
    }
}