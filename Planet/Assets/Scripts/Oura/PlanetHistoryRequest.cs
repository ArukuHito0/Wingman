using System;
using System.Collections.Generic;

[Serializable]
public class PlanetHistoryRequest
{
    public List<PlanetHistoryData> history;

    public PlanetHistoryRequest(
        List<PlanetHistoryData> history
    )
    {
        this.history = history;
    }
}