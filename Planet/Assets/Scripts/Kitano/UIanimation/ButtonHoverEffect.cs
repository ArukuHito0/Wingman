using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    [Header("拡大倍率")]
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);

    [Header("発光オブジェクト")]
    public GameObject glowObject;

    [Header("ホバーSE")]
    public AudioClip hoverSE;

    [Header("クリックSE")]
    public AudioClip clickSE;

    [Header("AudioSource")]
    public AudioSource audioSource;

    private Vector3 defaultScale;

    void Start()
    {
        // 元サイズ保存
        defaultScale = transform.localScale;

        // 発光を最初はOFF
        if (glowObject != null)
        {
            glowObject.SetActive(false);
        }
    }

    // カーソルが乗った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 拡大
        transform.localScale = hoverScale;

        // 発光ON
        if (glowObject != null)
        {
            glowObject.SetActive(true);
        }

        // ホバーSE
        if (hoverSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSE);
        }
    }

    // カーソルが離れた時
    public void OnPointerExit(PointerEventData eventData)
    {
        // 元サイズ
        transform.localScale = defaultScale;

        // 発光OFF
        if (glowObject != null)
        {
            glowObject.SetActive(false);
        }
    }

    // クリック時
    public void OnPointerClick(PointerEventData eventData)
    {
        // クリックSE
        if (clickSE != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSE);
        }
    }
}