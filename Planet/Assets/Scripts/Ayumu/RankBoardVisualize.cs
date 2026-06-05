using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class RankBoardVisualize : MonoBehaviour
{
    [SerializeField] private GameObject rankingLabelPrefab;
    [SerializeField] private Transform viewContent;

    [SerializeField] private int maxDisplayUsers = 10;

    private RankingLabel myRankingLabel;
    private List<RankingLabel> otherRankinglabelList = new List<RankingLabel>();

    private void Start()
    {
        UpdateRanking();
    }

    public void UpdateRanking()
    {
        StartCoroutine(DisplayRanking());
    }

    /// <summary>
    /// ランキング表示
    /// </summary>
    private IEnumerator DisplayRanking()
    {
        yield return GetMyRankingAPI();
        yield return GetRankingAPI();
    }

    /// <summary>
    /// 自分の順位を取得し、テキストに表示
    /// </summary>
    private IEnumerator GetMyRankingAPI()
    {
        WWWForm form = new WWWForm();
        form.AddField(FormFields.playerId, Matching.playerId);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("get_my_ranking"), form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                MyRanking myRanking = JsonUtility.FromJson<MyRanking>(www.downloadHandler.text);

                if (myRankingLabel == null)
                {
                    myRankingLabel = Instantiate(rankingLabelPrefab, viewContent).GetComponent<RankingLabel>();
                }

                myRankingLabel?.Initialize(myRanking.rank, myRanking.score, myRanking.name, UserIconSetting.Instance?.icons[myRanking.icon], true);
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

                RankingList rankingList = JsonUtility.FromJson<RankingList>(jsonText);

                int count = Mathf.Min(maxDisplayUsers, rankingList.users.Length);

                for (int i = 0; i < count; i++)
                {
                    int rank = i + 1;
                    string name = rankingList.users[i].user_name;
                    Sprite icon = UserIconSetting.Instance.icons[rankingList.users[i].user_icon];
                    int score = rankingList.users[i].best_score;

                    if (i >= otherRankinglabelList.Count)
                    {
                        RankingLabel ranking = Instantiate(rankingLabelPrefab, viewContent).GetComponent<RankingLabel>();
                        ranking.Initialize(rank, score, name, icon);

                        otherRankinglabelList.Add(ranking);
                    }
                    else
                    {
                        otherRankinglabelList[i].Initialize(rank, score, name, icon);
                    }
                }
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
        public int user_icon;
        public int best_score;
    }

    [System.Serializable]
    private class MyRanking
    {
        public int rank;
        public int score;
        public string name;
        public int icon;
    }
}
