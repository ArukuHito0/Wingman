using Unity.VisualScripting;
using UnityEngine;

public class FormFields
{
    /// <summary>
    /// 拡張子を除いた任意のファイル名を渡すとAPIを叩く為のURLを取得できる
    /// </summary>
    public static string GetFormURL(string fileName)
    {
        return "http://10.219.32.73/Planet_Backend/" + fileName + ".php";
    }

    public static readonly string password = "password";
    public static readonly string userId = "user_id";
    public static readonly string playerId = "player_id";
    public static readonly string roomId = "room_id";
    public static readonly string score = "score";
}
