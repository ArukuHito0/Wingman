using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;

    [Header("遷移Image")]
    public Image transitionImage;

    [Header("スライド速度")]
    public float slideSpeed = 2000f;

    [Header("中央停止時間")]
    public float centerWaitTime = 0.2f;

    [Header("拡大時回転速度")]
    public float expandRotateSpeed = 720f;

    [Header("縮小時回転速度")]
    public float shrinkRotateSpeed = 1080f;

    [Header("拡大速度")]
    public float expandSpeed = 40f;

    [Header("縮小速度")]
    public float shrinkSpeed = 40f;

    [Header("画面を覆う余裕倍率")]
    public float screenCoverMultiplier = 1.2f;

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

    public void StartTransition(
        string sceneName,
        Sprite sprite,
        RectTransform buttonRect,
        GameObject buttonObject
    )
    {
        if (isTransitioning) return;

        StartCoroutine(
            TransitionCoroutine(
                sceneName,
                sprite,
                buttonRect,
                buttonObject
            )
        );
    }

    IEnumerator TransitionCoroutine(
        string sceneName,
        Sprite sprite,
        RectTransform buttonRect,
        GameObject buttonObject
    )
    {
        isTransitioning = true;

        transitionImage.gameObject.SetActive(true);

        transitionImage.sprite = sprite;

        RectTransform rect = transitionImage.rectTransform;

        // ===== 初期設定 =====

        Vector3 buttonPos = buttonRect.position;

        rect.position = buttonPos;

        rect.sizeDelta = buttonRect.sizeDelta;

        rect.localScale = Vector3.one;

        rect.rotation = Quaternion.identity;

        // ===== ボタン下移動 =====

        Vector3 buttonTargetPos =
            buttonPos + Vector3.down * Screen.height;

        // ===== TransitionImage下スタート =====

        Vector3 startPos =
            buttonPos + Vector3.down * Screen.height;

        rect.position = startPos;

        // ===== 中央位置 =====

        Vector3 centerPos =
            new Vector3(
                Screen.width / 2f,
                Screen.height / 2f,
                0f
            );

        // ===== スライド =====

        while (
            Vector3.Distance(rect.position, centerPos) > 10f
        )
        {
            // ボタンを下へ
            buttonRect.position = Vector3.MoveTowards(
                buttonRect.position,
                buttonTargetPos,
                slideSpeed * Time.deltaTime
            );

            // Imageを中央へ
            rect.position = Vector3.MoveTowards(
                rect.position,
                centerPos,
                slideSpeed * Time.deltaTime
            );

            yield return null;
        }

        // 中央固定
        rect.position = centerPos;

        // ボタン非表示
        buttonObject.SetActive(false);

        // 少し停止
        yield return new WaitForSeconds(centerWaitTime);

        // ===== 必要scale計算 =====

        float screenDiagonal =
            Mathf.Sqrt(
                Screen.width * Screen.width +
                Screen.height * Screen.height
            );

        float imageSize =
            Mathf.Max(
                rect.rect.width,
                rect.rect.height
            );

        float targetScale =
            (screenDiagonal / imageSize)
            * screenCoverMultiplier;

        // ===== 拡大 =====

        while (rect.localScale.x < targetScale)
        {
            rect.Rotate(
                0f,
                0f,
                expandRotateSpeed * Time.deltaTime
            );

            rect.localScale = Vector3.MoveTowards(
                rect.localScale,
                Vector3.one * targetScale,
                expandSpeed * Time.deltaTime
            );

            yield return null;
        }

        rect.localScale = Vector3.one * targetScale;

        // ===== シーン移動 =====

        SceneManager.LoadScene(sceneName);

        // 1フレーム待つ
        yield return null;

        // RectTransform再取得
        rect = transitionImage.rectTransform;

        // 少し待つ
        yield return new WaitForSeconds(waitTime);

        // ===== 縮小 =====

        while (rect.localScale.x > 0.05f)
        {
            rect.Rotate(
                0f,
                0f,
                -shrinkRotateSpeed * Time.deltaTime
            );

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