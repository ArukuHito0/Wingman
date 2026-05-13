using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlanetHistoryAPI : MonoBehaviour
{
    public string apiURL =
     "http://localhost/Planet_Buckend/save_history.php";

    public void SendHistory()
    {
        StartCoroutine(
            SendHistoryCoroutine()
        );
    }

    private IEnumerator SendHistoryCoroutine()
    {
        // -------------------
        // JSON化
        // -------------------

        PlanetHistoryRequest requestData =
            new PlanetHistoryRequest(
                PlanetHistoryManager
                .Instance
                .history
            );

        string json =
            JsonUtility.ToJson(
                requestData
            );

        Debug.Log(json);

        // -------------------
        // フォーム作成
        // -------------------

        WWWForm form =
            new WWWForm();

        form.AddField(
            "planet_history",
            json
        );

        form.AddField(
            "player_id",
            Matching.playerId
        );

        // -------------------
        // POST送信
        // -------------------

        using (
            UnityWebRequest www =
            UnityWebRequest.Post(
                apiURL,
                form
            )
        )
        {
            www.timeout = 10;

            yield return
                www.SendWebRequest();

            if (
                www.result ==
                UnityWebRequest.Result.Success
            )
            {
                Debug.Log(
                    "送信成功: " +
                    www.downloadHandler.text
                );
            }
            else
            {
                Debug.LogError(
                    "送信失敗: " +
                    www.error
                );
            }
        }
    }
}