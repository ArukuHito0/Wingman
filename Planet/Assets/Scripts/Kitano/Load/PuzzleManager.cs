using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public void StartBattle()
    {
        Debug.Log("攻撃データ送信");

        SceneTransition.Load(SceneNames.Shooting);
    }
}