using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UISlideTransition_Left : MonoBehaviour
{
    [Header("右へスライドアウトするUI")]
    public List<RectTransform> slideOutUI;

    [Header("左からスライドインするUI")]
    public List<RectTransform> slideInUI;

    [Header("移動時間")]
    public float moveTime = 0.5f;

    [Header("画面外へ移動する距離")]
    public float slideDistance = 2000f;

    private bool isMoving = false;

    // 元位置保存
    private Dictionary<RectTransform, Vector2> originalPositions =
        new Dictionary<RectTransform, Vector2>();

    void Start()
    {
        // OUT側の元位置保存
        foreach (RectTransform ui in slideOutUI)
        {
            originalPositions[ui] = ui.anchoredPosition;
        }

        // IN側の元位置保存
        foreach (RectTransform ui in slideInUI)
        {
            originalPositions[ui] = ui.anchoredPosition;

            // 最初は左画面外へ
            Vector2 pos = ui.anchoredPosition;
            pos.x -= slideDistance;

            ui.anchoredPosition = pos;
        }
    }

    // 開く
    public void Open()
    {
        if (!isMoving)
        {
            StartCoroutine(OpenAnimation());
        }
    }

    // 戻る
    public void Back()
    {
        if (!isMoving)
        {
            StartCoroutine(BackAnimation());
        }
    }

    IEnumerator OpenAnimation()
    {
        isMoving = true;

        float time = 0f;

        Dictionary<RectTransform, Vector2> outStart =
            new Dictionary<RectTransform, Vector2>();

        Dictionary<RectTransform, Vector2> outEnd =
            new Dictionary<RectTransform, Vector2>();

        Dictionary<RectTransform, Vector2> inStart =
            new Dictionary<RectTransform, Vector2>();

        Dictionary<RectTransform, Vector2> inEnd =
            new Dictionary<RectTransform, Vector2>();

        // OUT（右へ）
        foreach (RectTransform ui in slideOutUI)
        {
            Vector2 start = ui.anchoredPosition;
            Vector2 end = start;

            end.x += slideDistance;

            outStart[ui] = start;
            outEnd[ui] = end;
        }

        // IN（左から中央へ）
        foreach (RectTransform ui in slideInUI)
        {
            Vector2 start = ui.anchoredPosition;
            Vector2 end = originalPositions[ui];

            inStart[ui] = start;
            inEnd[ui] = end;
        }

        while (time < moveTime)
        {
            time += Time.deltaTime;

            float t = time / moveTime;
            t = Mathf.SmoothStep(0f, 1f, t);

            // OUT
            foreach (RectTransform ui in slideOutUI)
            {
                ui.anchoredPosition =
                    Vector2.Lerp(outStart[ui], outEnd[ui], t);
            }

            // IN
            foreach (RectTransform ui in slideInUI)
            {
                ui.anchoredPosition =
                    Vector2.Lerp(inStart[ui], inEnd[ui], t);
            }

            yield return null;
        }

        isMoving = false;
    }

    IEnumerator BackAnimation()
    {
        isMoving = true;

        float time = 0f;

        Dictionary<RectTransform, Vector2> outStart =
            new Dictionary<RectTransform, Vector2>();

        Dictionary<RectTransform, Vector2> outEnd =
            new Dictionary<RectTransform, Vector2>();

        Dictionary<RectTransform, Vector2> inStart =
            new Dictionary<RectTransform, Vector2>();

        Dictionary<RectTransform, Vector2> inEnd =
            new Dictionary<RectTransform, Vector2>();

        // 元UIを戻す（右から）
        foreach (RectTransform ui in slideOutUI)
        {
            Vector2 start = ui.anchoredPosition;
            Vector2 end = originalPositions[ui];

            outStart[ui] = start;
            outEnd[ui] = end;
        }

        // Popupを左へ戻す
        foreach (RectTransform ui in slideInUI)
        {
            Vector2 start = ui.anchoredPosition;
            Vector2 end = start;

            end.x -= slideDistance;

            inStart[ui] = start;
            inEnd[ui] = end;
        }

        while (time < moveTime)
        {
            time += Time.deltaTime;

            float t = time / moveTime;
            t = Mathf.SmoothStep(0f, 1f, t);

            // 戻る側
            foreach (RectTransform ui in slideOutUI)
            {
                ui.anchoredPosition =
                    Vector2.Lerp(outStart[ui], outEnd[ui], t);
            }

            // 消える側
            foreach (RectTransform ui in slideInUI)
            {
                ui.anchoredPosition =
                    Vector2.Lerp(inStart[ui], inEnd[ui], t);
            }

            yield return null;
        }

        isMoving = false;
    }
}