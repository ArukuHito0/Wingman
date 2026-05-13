using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void GoToShootingPhase()
    {
        Debug.Log("GoToShootingPhase");

        SceneTransition.Load("Shooting Phase");
    }
}