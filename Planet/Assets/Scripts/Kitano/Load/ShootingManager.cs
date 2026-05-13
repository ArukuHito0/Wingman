using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayBGM("Shooting");
    }

    public void ReturnPuzzle()
    {
        Debug.Log("ラウンド終了");

        SceneTransition.Load(SceneNames.Puzzle);
    }
}