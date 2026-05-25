using UnityEngine;

public class TitleManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayBGM("Title");
    }
}