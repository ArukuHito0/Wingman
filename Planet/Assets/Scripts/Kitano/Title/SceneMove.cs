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

        if (button == null)
        {
            Debug.LogError("Buttonコンポーネントがありません！");
            return;
        }

        design = GetComponent<RandomButtonDesign>();

        if (design == null)
        {
            Debug.LogError("RandomButtonDesignがありません！");
            return;
        }

        button.onClick.AddListener(GoToGameScene);
    }

    public void GoToGameScene()
    {
        button.interactable = false;

        SceneTransitionManager.instance.StartTransition(
            nextSceneName,
            design.selectedSprite,
            GetComponent<RectTransform>(),
            gameObject
        );
    }
}