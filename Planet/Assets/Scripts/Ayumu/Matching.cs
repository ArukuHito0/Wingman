using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebGLSupport;

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

    public static string userName
    {
        get
        {
            return PlayerPrefs.GetString("UserName", "");
        }
        private set
        {
            PlayerPrefs.SetString("UserName", value);
            PlayerPrefs.Save();
        }
    }

    private bool isLoggedIn = false;

    [SerializeField]
    private TMP_InputField userNameIF;

    private void Awake()
    {
        StartCoroutine(Login());
    }

    private void Start()
    {
        userNameIF.text = userName;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void SetUserName(string userName)
    {
        Matching.userName = userName;
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
                Debug.Log("ログイン失敗");
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
                Debug.Log("通信失敗");
            }
        }
    }
}
