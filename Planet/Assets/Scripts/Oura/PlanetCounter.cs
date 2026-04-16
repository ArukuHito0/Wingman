using UnityEngine;

public class PlanetCounter : MonoBehaviour
{
    public static PlanetCounter Instance;

    public int[] counts = new int[8]; // 0〜7

    void Awake()
    {
        Instance = this;
    }

    public void Add(int level)
    {
        counts[level]++;
    }
}