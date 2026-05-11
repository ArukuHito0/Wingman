using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    public void ReturnPuzzle()
    {
        Debug.Log("ラウンド終了");

        SceneTransition.Load(SceneNames.Puzzle);
    }
}