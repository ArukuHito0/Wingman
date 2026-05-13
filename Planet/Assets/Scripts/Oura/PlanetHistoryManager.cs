using UnityEngine;
using System.Collections.Generic;

public class PlanetHistoryManager : MonoBehaviour
{
    public static PlanetHistoryManager Instance;

    public List<PlanetHistoryData> history =
        new List<PlanetHistoryData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddHistory(int level)
    {
        float currentTime = Time.time;

        history.Add(
            new PlanetHistoryData(level, currentTime)
        );

        Debug.Log(
            "履歴追加: " +
            level +
            " 時間:" +
            currentTime
        );
    }
}