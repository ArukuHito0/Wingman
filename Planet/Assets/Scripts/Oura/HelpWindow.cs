using UnityEngine;

public class HelpWindow : MonoBehaviour
{
    [SerializeField] private GameObject helpRoot;
    [SerializeField] private GameObject gameUI;

    public void OpenHelp()
    {
        gameUI.SetActive(false);
        helpRoot.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CloseHelp()
    {
        helpRoot.SetActive(false);
        gameUI.SetActive(true);

        Time.timeScale = 1f;
    }
}