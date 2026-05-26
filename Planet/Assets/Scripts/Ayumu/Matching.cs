using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// ルームの状態を表す定数をまとめたクラス
/// </summary>
public class GameStatus
{
    public const string WAITING = "waiting";
    public const string PLAYING = "playing";
    public const string FINISHED = "finished";
}

/// <summary>
/// マッチングに関しての関数をまとめたクラス
/// </summary>
public class Matching : MonoBehaviour
{
    private class MatchingResponse
    {
        public string user_id;
        public int player_id;
    }

    /// <summary>
    /// DBでデータ検索に使用する連番IDプロパティ
    /// </summary>
    public static int playerId { get; private set; }

    /// <summary>
    /// PHPから発行されたユーザーIDを保存しておくプロパティ
    /// </summary>
    public static string userId
    {
        get
        {
            return PlayerPrefs.GetString("UserID", "");
        }
        private set
        {
            PlayerPrefs.SetString("UserID", value);
            PlayerPrefs.Save();
        }
    }

    private string userName = string.Empty;

    public void SetUserName(string userName)
    {
        this.userName = userName;
    }
}
