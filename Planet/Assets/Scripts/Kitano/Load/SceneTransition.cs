using UnityEngine.SceneManagement;

public static class SceneTransition
{
    public static void Load(string nextScene)
    {
        LoadingManager.nextScene = nextScene;

        SceneManager.LoadScene(SceneNames.Loading);
    }
}