using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GravityGauge : MonoBehaviour
{
    private PlayerController player;
    private Image gauge;
    private Image frame;

    private RectTransform framRect;
    private Color baseFrameColor;
    private Vector3 baseFrameScale;
    [SerializeField]
    private Vector3 afterFrameScale;
    [SerializeField]
    private float animationTime;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        gauge = transform.Find("Background").Find("Gauge").GetComponent<Image>();
        frame = transform.Find("Background").Find("FrameEffect").GetComponent<Image>();
        framRect = frame.GetComponent<RectTransform>();
        baseFrameColor = frame.color;
        baseFrameScale = frame.rectTransform.localScale;
    }

    private void OnEnable()
    {
        player.onUpdateCoolTime += UpdateGaugeFillAmount;
    }

    private void OnDisable()
    {
        player.onUpdateCoolTime -= UpdateGaugeFillAmount;
    }

    private void UpdateGaugeFillAmount(float ratio)
    {
        gauge.fillAmount = ratio;

        if (ratio >= 1)
        {
            StartCoroutine(Animation());
        }
    }

    private IEnumerator Animation()
    {
        float time = 0;
        float alpha = 0;
        var color = new Color(0, 0, 0, 0);

        while (time < animationTime)
        {
            time += Time.deltaTime;

            alpha = Mathf.Lerp(baseFrameColor.a, color.a, time / animationTime);
            frame.color = new Color(baseFrameColor.r, baseFrameColor.g, baseFrameColor.b, alpha);
            framRect.localScale = Vector3.Lerp(baseFrameScale, afterFrameScale, time / animationTime);

            yield return null;
        }
    }
}
