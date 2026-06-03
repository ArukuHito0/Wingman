using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// マッチングに関しての関数をまとめたクラス
/// </summary>
public class Matching : MonoBehaviour
{
    class UserDataResponse
    {
        public int player_id;
        public string user_id;
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

    private bool isLoggedIn = false;

    private void Awake()
    {
        StartCoroutine(Login());
    }

    public void SetUserName(string userName)
    {
        this.userName = userName;
        StartCoroutine(SetUserName());
    }

    private IEnumerator Login()
    {
        Debug.Log("コルーチンを開始");

        WWWForm form = new WWWForm();
        form.AddField(FormFields.userId, userId);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("login"), form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("ログイン成功");

                UserDataResponse response = JsonUtility.FromJson<UserDataResponse>(www.downloadHandler.text);
                userId = response.user_id;
                playerId = response.player_id;

                isLoggedIn = true;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                yield return Login();
            }
        }
    }

    private IEnumerator SetUserName()
    {
        WWWForm form = new WWWForm();
        form.AddField(FormFields.userId, userId);
        form.AddField(FormFields.userName, userName);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("set_user_name"), form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("DBの名前を更新");
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                yield return SetUserName();
            }
        }
    }
}
