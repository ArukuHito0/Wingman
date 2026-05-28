using UnityEngine;
using static CursorManager;

public static class CursorChanger
{
    /// <summary>
    /// カーソルの見た目を変更
    /// </summary>
    /// <param name="mode"></param>
    public static void SetCursorTexture(CursorType mode)
    {
        CursorManager.Instance.SetCursorTexture(mode);
    }
}

public class CursorManager : MonoBehaviour
{
    public enum CursorType
    {
        Default,
        Movement,
        Skill,
    }

    public static CursorManager Instance { get; private set; }

    public Texture2D defaultCursor;
    public Texture2D movementCursor;
    public Texture2D skillCursor;

    public Vector2 hotSpot = Vector2.zero;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetCursorTexture(CursorType.Default);
    }

    public void SetCursorTexture(CursorType mode)
    {
        Texture2D texture = defaultCursor;
        Vector2 hotSpot = this.hotSpot;

        switch (mode)
        {
            case CursorType.Default:
                texture = defaultCursor;
                break;
            case CursorType.Movement:
                texture = movementCursor;
                break;
            case CursorType.Skill:
                texture = skillCursor;
                hotSpot = new Vector2(skillCursor.width / 2, skillCursor.height / 2);
                break;
        }

        Cursor.SetCursor(texture, hotSpot, CursorMode.Auto);
    }
}
