using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonSceneTransition : MonoBehaviour
{
    [Header("遷移先シーン名")]
    public string nextSceneName;

    [Header("画面を覆うImage")]
    public Image transitionImage;

    [Header("回転速度")]
    public float rotateSpeed = 720f;

    [Header("拡大速度")]
    public float expandSpeed = 8f;

    [Header("縮小速度")]
    public float shrinkSpeed = 10f;

    [Header("最終サイズ")]
    public float targetScale = 25f;

    private bool isTransitioning = false;

    private RandomButtonDesign designManager;
    private Button button;

    void Start()
    {
        designManager = GetComponent<RandomButtonDesign>();
        button = GetComponent<Button>();

        transitionImage.gameObject.SetActive(false);
    }

    public void StartTransition()
    {
        if (isTransitioning) return;

        isTransitioning = true;

        button.interactable = false;

        StartCoroutine(TransitionCoroutine());
    }

    IEnumerator TransitionCoroutine()
    {
        transitionImage.gameObject.SetActive(true);

        // ボタンと同じ見た目にする
        transitionImage.sprite = designManager.selectedSprite;

        RectTransform rect = transitionImage.rectTransform;

        rect.localScale = Vector3.zero;
        rect.rotation = Quaternion.identity;

        while (rect.localScale.x < targetScale)
        {
            rect.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);

            rect.localScale = Vector3.MoveTowards(
                rect.localScale,
                Vector3.one * targetScale,
                expandSpeed * Time.deltaTime * targetScale
            );

            yield return null;
        }

        rect.localScale = Vector3.one * targetScale;

        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(nextSceneName);

        // シーン遷移
        SceneManager.LoadScene(nextSceneName);
    }
}