using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// マッチングに関しての関数をまとめたクラス
/// </summary>
public class Matching : MonoBehaviour
{
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
