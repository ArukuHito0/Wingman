using UnityEngine;
using TMPro;

public class PlanetCounterUI : MonoBehaviour
{
    public TextMeshProUGUI[] texts; // 8個

    void Update()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = PlanetCounter.Instance.counts[i].ToString();
        }
    }
}