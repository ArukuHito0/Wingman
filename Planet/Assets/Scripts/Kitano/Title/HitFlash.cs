using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [Header("接触判定するタグ")]
    [SerializeField] private string targetTag = "Planet";

    [Header("一瞬表示するオブジェクト")]
    [SerializeField] private GameObject flashObject;

    [Header("表示時間")]
    [SerializeField] private float flashTime = 0.1f;

    private Coroutine flashCoroutine;

    private void Start()
    {
        Debug.Log("HitFlash Start");

        if (flashObject != null)
        {
            flashObject.SetActive(false);
            Debug.Log("flashObject を非表示にしました");
        }
        else
        {
            Debug.LogWarning("flashObject が設定されていません");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("2D接触 : " + other.name);

        if (other.CompareTag(targetTag))
        {
            Debug.Log("指定タグと接触 : " + targetTag);

            StartFlash();
        }
        else
        {
            Debug.Log("タグ不一致 : " + other.tag);
        }
    }

    private void StartFlash()
    {
        Debug.Log("フラッシュ開始");

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            Debug.Log("前回のフラッシュ停止");
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        Debug.Log("flashObject 表示");

        flashObject.SetActive(true);

        yield return new WaitForSeconds(flashTime);

        flashObject.SetActive(false);

        Debug.Log("flashObject 非表示");
    }
}