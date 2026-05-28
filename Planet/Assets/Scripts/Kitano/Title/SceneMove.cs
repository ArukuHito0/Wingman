using UnityEngine;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    [Header("遷移先")]
    public string nextSceneName = "Shooting Phase";

    private Button button;
    private RandomButtonDesign design;

    void Start()
    {
        button = GetComponent<Button>();

        design = GetComponent<RandomButtonDesign>();

        button.onClick.AddListener(GoToGameScene);
    }

    public void GoToGameScene()
    {
        button.interactable = false;

        SceneTransitionManager.instance.StartTransition(
            nextSceneName,
            design.selectedSprite
        );
    }
}