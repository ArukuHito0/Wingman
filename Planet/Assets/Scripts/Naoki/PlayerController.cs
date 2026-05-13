using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [UnitHeaderInspectable("移動設定")]
    public float acceleration = 10f;
    private float maxSpeed = 10f;

    [Header("参照")]
    public Rigidbody2D rb;
    public ShootingController shooter;

    [Header("移動制限設定")]
    private float moveLimitX = 7.11f;
    private float moveLimitY = 4f;

    // 内部計算用
    private Vector2 targetDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // マウスのワールド座標を取得
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Z軸を0に固定
        mousePos.z = 0;

        // 方向ベクトル計算
        Vector2 direction = new Vector2
            (
                mousePos.x - transform.position.x,
                mousePos.y - transform.position.y
            );

        // ラジアン -> 度数法
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Z軸を回転
        transform.rotation = Quaternion.Euler(0, 0, angle);


        // 1. 左クリックしている間だけ進む方向を更新
        if (Input.GetMouseButton(0))
        {
            targetDirection = ((Vector2)mousePos - rb.position).normalized;
        }
    }

    private void FixedUpdate()
    {
        // スペースキーが押されている間だけ加速
        if (Input.GetMouseButton(0))
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
            //float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg - 90f;
            //rb.rotation = angle;
        }

        // 現在の速度を一時保存
        Vector2 currentVelocity = rb.linearVelocity;

        // X方向の制限
        if (transform.position.x >= moveLimitX && currentVelocity.x > 0)
        {
            currentVelocity.x = 0;
        }
        else if (transform.position.x <= -moveLimitX && currentVelocity.x < 0)
        {
            currentVelocity.x = 0;
        }
        // Y方向の制限
        if (transform.position.y >= moveLimitY && currentVelocity.y > 0)
        {
            currentVelocity.y = 0;
        }
        else if (transform.position.y <= -moveLimitY && currentVelocity.y < 0)
        {
            currentVelocity.y = 0;
        }

        // 修正した速度をrbに戻す
        rb.linearVelocity = currentVelocity;


        // 現在の座標を取り出す
        Vector3 currentPos = transform.position;

        // X座標を制限 (-moveLimitX から moveLimitX の間に収める)
        currentPos.x = Mathf.Clamp(currentPos.x, -moveLimitX, moveLimitX);
        // Y座標を制限 (-moveLimitY から moveLimitY の間に収める)
        currentPos.y = Mathf.Clamp(currentPos.y, -moveLimitY, moveLimitY);

        // 制限した値をプレイヤーの座標に戻す
        transform.position = currentPos;
    }

    private void RestrictMovement()
    {

    }

    private void ClampVelocityAtBoundaries()
    {

    }
}