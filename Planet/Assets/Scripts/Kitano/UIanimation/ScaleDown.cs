using UnityEngine;

public class ScaleDown : MonoBehaviour
{
    [Header("Scale Settings")]
    public Vector3 targetScale = Vector3.zero; // 最終サイズ（どこまで縮めるか）
    public float shrinkSpeed = 1f;             // 縮小スピード

    [Header("Timing")]
    public float startDelay = 0f; // 縮小開始までの時間

    private Vector3 initialScale;
    private float timer = 0f;
    private bool isShrinking = false;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 開始時間を過ぎたら縮小開始
        if (!isShrinking && timer >= startDelay)
        {
            isShrinking = true;
        }

        if (isShrinking)
        {
            transform.localScale = Vector3.MoveTowards(
                transform.localScale,
                targetScale,
                shrinkSpeed * Time.deltaTime
            );
        }
    }
}