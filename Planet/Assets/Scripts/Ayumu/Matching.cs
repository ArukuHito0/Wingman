using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GameStatus
{
    public const string WAITING = "waiting";
    public const string PLAYING = "playing";
    public const string FINISHED = "finished";
}

public class Matching : MonoBehaviour
{
    private class MatchingResponse
    {
        public string user_id;
        public int player_id;
        public int room_id;
    }

    public static int roomId {  get; private set; }
    
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

    private string password = "";

    public void SetRoomPassword(string password)
    {
        this.password = password;
    }

    public void OnClickMatching()
    {
        StartCoroutine(MatchingAPI());
    }

    /// <summary>
    /// ルームへの参加または作成のAPIを叩くコルーチン
    /// </summary>
    private IEnumerator MatchingAPI()
    {
        WWWForm form = new WWWForm();
        form.AddField(FormFields.password, password);
        form.AddField(FormFields.userId, userId);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("join_or_create_room"), form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(www.downloadHandler.text);

                MatchingResponse response = JsonUtility.FromJson<MatchingResponse>(www.downloadHandler.text);
                userId = response.user_id;
                playerId = response.player_id;
                roomId = response.room_id;

                StartCoroutine(WaintingAPI());
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                yield return MatchingAPI();
            }
        }
    }

    /// <summary>
    /// マッチング待機のAPIを叩くコルーチン
    /// </summary>
    private IEnumerator WaintingAPI()
    {
        while (true)
        {
            if(roomId != 0)
            {
                WWWForm form = new WWWForm();
                form.AddField(FormFields.roomId, roomId);

                using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("wait_matching"), form))
                {
                    yield return www.SendWebRequest();

                    if (www.result == UnityWebRequest.Result.Success)
                    {
                        string response = www.downloadHandler.text;

                        if (response == GameStatus.PLAYING)
                        {
                            Debug.Log("プレイヤーが揃いました");
                            yield break;
                        }
                    }
                }
            }

            yield return null;
        }
    }
}
