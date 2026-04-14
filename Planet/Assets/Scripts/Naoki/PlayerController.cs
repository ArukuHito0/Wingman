using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [UnitHeaderInspectable("移動設定")]
    public float acceleration = 10f;
    private float maxSpeed = 10f;

    [Header("参照")]
    public Rigidbody2D rb;
    public ShootingController shooter;

    // 内部計算用
    private Vector2 targetDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // マウス座標を取得
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Z軸を0に固定
        mousePos.z = 0;

        // 1. スペースキーを押している間だけ進む方向を更新
        if (Input.GetKey(KeyCode.Space))
        {
            targetDirection = ((Vector2)mousePos - rb.position).normalized;
        }

        // 2. 左クリックで発射
        if (Input.GetMouseButtonDown(0))
        {
            shooter.bulletPool.Get(); // プールから弾を出す
        }
    }

    private void FixedUpdate()
    {
        // スペースキーが押されている間だけ加速
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(targetDirection * acceleration);
        }

        // 速度を最高速度いかに制限する
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // 進んでいる方向にキャラクターを回転させる
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            // 進行方向(Velocity)を向くための角度計算
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
    }
}