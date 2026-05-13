using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("PuzzleManagerが起動しました。");
        AudioManager.instance.PlayBGM("Puzzle");
    }

    public void StartBattle()
    {
        Debug.Log("攻撃データ送信");

        SceneTransition.Load(SceneNames.Shooting);
    }
}