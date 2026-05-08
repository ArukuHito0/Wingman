using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    public float rotateSpeed = 0f;
    public float targetSpeed = 720f; // ゲームオーバー時の回転速度
    public float accel = 600f;

    public bool active = false;


    Vector3 defaultScale;

    void Start()
    {
        defaultScale = transform.localScale;
    }

    void Update()
    {
        if (active)
        {
            // 回転加速
            rotateSpeed = Mathf.MoveTowards(
                rotateSpeed,
                targetSpeed,
                accel * Time.deltaTime
            );

            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

            // 少し脈動
            float size = 1f + Mathf.Sin(Time.time * 8f) * 0.05f;
            transform.localScale = defaultScale * size;
        }
    }

    public void Activate()
    {
        active = true;
    }

    public void ResetHole()
    {
        active = false;
        rotateSpeed = 0f;
        transform.rotation = Quaternion.identity;
        transform.localScale = defaultScale;
    }
}