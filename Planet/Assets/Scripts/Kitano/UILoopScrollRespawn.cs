using UnityEngine;

public class UILoopScrollRespawn : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    [Header("移動設定")]
    public Direction direction = Direction.Left;
    public float speed = 100f;

    [Header("位置指定")]
    public float despawnPosition; // 到達したら消える位置（X or Y）
    public float respawnPosition; // 復活する位置（X or Y）

    private RectTransform rect;
    private Vector2 moveDir;

    void Start()
    {
        rect = GetComponent<RectTransform>();

        switch (direction)
        {
            case Direction.Left: moveDir = Vector2.left; break;
            case Direction.Right: moveDir = Vector2.right; break;
            case Direction.Up: moveDir = Vector2.up; break;
            case Direction.Down: moveDir = Vector2.down; break;
        }
    }

    void Update()
    {
        rect.anchoredPosition += moveDir * speed * Time.deltaTime;

        Vector2 pos = rect.anchoredPosition;

        // 方向ごとに判定
        switch (direction)
        {
            case Direction.Left:
                if (pos.x <= despawnPosition)
                    pos.x = respawnPosition;
                break;

            case Direction.Right:
                if (pos.x >= despawnPosition)
                    pos.x = respawnPosition;
                break;

            case Direction.Up:
                if (pos.y >= despawnPosition)
                    pos.y = respawnPosition;
                break;

            case Direction.Down:
                if (pos.y <= despawnPosition)
                    pos.y = respawnPosition;
                break;
        }

        rect.anchoredPosition = pos;
    }
}