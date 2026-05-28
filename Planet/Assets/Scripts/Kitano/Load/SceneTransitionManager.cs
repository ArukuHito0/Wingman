using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    [Header("遷移Image")]
    public Image transitionImage;

    [Header("拡大時回転速度")]
    public float expandRotateSpeed = 90f;

    [Header("縮小時回転速度")]
    public float shrinkRotateSpeed = 120f;

    [Header("拡大速度")]
    public float expandSpeed = 40f;

    [Header("縮小速度")]
    public float shrinkSpeed = 40f;

    [Header("画面を覆うサイズ")]
    public float targetScale = 25f;

    [Header("縮小開始前待機")]
    public float waitTime = 0.3f;

    bool isTransitioning = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        transitionImage.gameObject.SetActive(false);
    }

    public void StartTransition(string sceneName, Sprite sprite)
    {
        if (isTransitioning) return;

        StartCoroutine(TransitionCoroutine(sceneName, sprite));
    }

    IEnumerator TransitionCoroutine(string sceneName, Sprite sprite)
    {
        isTransitioning = true;

        transitionImage.gameObject.SetActive(true);

        transitionImage.sprite = sprite;

        RectTransform rect = transitionImage.rectTransform;

        rect.localScale = Vector3.zero;

        rect.rotation = Quaternion.identity;

        // ===== 拡大 =====
        while (rect.localScale.x < targetScale)
        {
            rect.Rotate(0f, 0f, expandRotateSpeed * Time.deltaTime);

            rect.localScale = Vector3.MoveTowards(
                rect.localScale,
                Vector3.one * targetScale,
                expandSpeed * Time.deltaTime
            );

            yield return null;
        }

        rect.localScale = Vector3.one * targetScale;

        // シーン移動
        SceneManager.LoadScene(sceneName);

        // 少し待つ
        yield return new WaitForSeconds(waitTime);

        // ===== 縮小 =====
        while (rect.localScale.x > 0.05f)
        {
            // 逆回転
            rect.Rotate(0f, 0f, -shrinkRotateSpeed * Time.deltaTime);

            rect.localScale = Vector3.MoveTowards(
                rect.localScale,
                Vector3.zero,
                shrinkSpeed * Time.deltaTime
            );

            yield return null;
        }

        rect.localScale = Vector3.zero;

        transitionImage.gameObject.SetActive(false);

        isTransitioning = false;
    }
}