using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("回転設定")]
    public Vector3 rotationAxis = new Vector3(0, 1, 0); // 回転軸
    public float rotationSpeed = 90f; // 度/秒

    [Header("タイミング")]
    public float startDelay = 0f; // 回転開始までの時間

    private float timer = 0f;
    private bool isRotating = false;

    void Update()
    {
        timer += Time.deltaTime;

        // 開始時間を過ぎたら回転開始
        if (!isRotating && timer >= startDelay)
        {
            isRotating = true;
        }

        if (isRotating)
        {
            transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);
        }
    }
}