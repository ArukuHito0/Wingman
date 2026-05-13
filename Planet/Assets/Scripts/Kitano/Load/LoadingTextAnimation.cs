using UnityEngine;
using TMPro;

public class LoadingTextAnimation : MonoBehaviour
{
    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        InvokeRepeating(nameof(Animate), 0f, 0.5f);
    }

    int dotCount = 0;

    void Animate()
    {
        dotCount++;

        if (dotCount > 3)
            dotCount = 0;

        text.text = "通信中" + new string('.', dotCount);
    }
}