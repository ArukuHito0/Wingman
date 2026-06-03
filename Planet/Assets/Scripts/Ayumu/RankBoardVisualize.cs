using System.Collections;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class RankBoardVisualize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myRankText;
    [SerializeField] private TextMeshProUGUI myScoreText;
    [SerializeField] private TextMeshProUGUI rankingBoardText;

    [SerializeField] private int maxDisplayUsers = 99;

    private void Start()
    {
        DisplayMyInfo();
        DisplayRanking();
    }

    /// <summary>
    /// 自分の順位とハイスコアの表示
    /// </summary>
    public void DisplayMyInfo()
    {
        StartCoroutine(GetMyRankingAPI());
        StartCoroutine(GetMyBestScoreAPI());
    }

    /// <summary>
    /// ランキング表示
    /// </summary>
    public void DisplayRanking()
    {
        StartCoroutine(GetRankingAPI());
    }

    /// <summary>
    /// 自分の順位を取得し、テキストに表示
    /// </summary>
    private IEnumerator GetMyRankingAPI()
    {
        WWWForm form = new WWWForm();
        form.AddField(FormFields.playerId, Matching.playerId);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("get_my_rank"), form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (int.TryParse(www.downloadHandler.text, out int rank))
                {
                    Debug.Log(rank);

                    if(rank == 0)
                        myRankText.text = $"? | あなた";
                    else
                        myRankText.text = $"{rank} | あなた";
                }
            }
            else
            {
                yield return GetMyRankingAPI();
            }
        }
    }

    /// <summary>
    /// 自分のハイスコアを取得し、テキストに表示
    /// </summary>
    private IEnumerator GetMyBestScoreAPI()
    {
        WWWForm form = new WWWForm();
        form.AddField(FormFields.playerId, Matching.playerId);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("get_score"), form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (int.TryParse(www.downloadHandler.text, out int best_score))
                {
                    if (best_score <= 0)
                    {
                        myScoreText.text = "------";
                    }
                    else
                    {
                        myScoreText.text = best_score.ToString();
                    }
                }
            }
            else
            {
                yield return GetMyBestScoreAPI();
            }
        }
    }

    /// <summary>
    /// 全体ランキングを取得して、ランキングボードに表示
    /// </summary>
    private IEnumerator GetRankingAPI()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(FormFields.GetFormURL("get_ranking")))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string jsonText = "{\"users\":" + www.downloadHandler.text + "}";

                RankingList ranking = JsonUtility.FromJson<RankingList>(jsonText);

                StringBuilder sb = new StringBuilder();

                int count = Mathf.Min(maxDisplayUsers, ranking.users.Length);

                for (int i = 0; i < count; i++)
                {
                    int rank = i + 1;
                    string name = ranking.users[i].user_name;
                    int score = ranking.users[i].best_score;

                    if (ranking.users[i].id == Matching.playerId)
                    {
                        if (rank < 10)
                            sb.Append($"<color=red> {rank} | {name} {score}\n</color>");
                        else
                            sb.Append($"<color=red>{rank} | {name} {score}\n</color>");
                    }
                    else
                    {
                        if (rank < 10)
                            sb.Append($" {rank} | {name} {score}\n");
                        else
                            sb.Append($"{rank} | {name} {score}\n");
                    }
                }

                rankingBoardText.text = sb.ToString();
            }
            else
            {
                yield return GetRankingAPI();
            }
        }
    }

    [System.Serializable]
    private class RankingList
    {
        public Ranking[] users;
    }

    [System.Serializable]
    private class Ranking
    {
        public int id;
        public string user_name;
        public int best_score;
    }
}
