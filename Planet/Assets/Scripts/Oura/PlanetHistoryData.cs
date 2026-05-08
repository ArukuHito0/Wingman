using System;

[Serializable]
public class PlanetHistoryData
{
    public int level;

    public float time;

    public PlanetHistoryData(int level, float time)
    {
        this.level = level;
        this.time = time;
    }
}