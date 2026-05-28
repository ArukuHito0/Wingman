using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private static int score { get; set; } = 0;
    private static int highScore {  get; set; }

    public TextMeshProUGUI scoreTitleText;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score = 0;
        UpdateUI();
    }

    // 加点
    public void AddScore(int value)
    {
        score += value;

        UpdateUI();
    }

    // 減点
    public void MinusScore(int value)
    {
        score -= value;

        // マイナス防止
        if (score < 0)
        {
            score = 0;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
#if UNITY_EDITOR
        return PlayerPrefs.GetInt("HighScore", 0);
#else
        StartCoroutine(GetScoreAPI());
        return highScore;
#endif
    }

    /// <summary>
    /// DB上のプレイヤーのスコアを更新するコルーチン
    /// </summary>
    public IEnumerator SendScoreAPI()
    {
        WWWForm form = new WWWForm();
        form.AddField(FormFields.playerId, Matching.playerId);
        form.AddField(FormFields.score, score);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("set_score"), form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                yield return SendScoreAPI();
            }
        }
    }

    /// <summary>
    /// DB上のプレイヤーのスコアを取得するコルーチン
    /// </summary>
    public IEnumerator GetScoreAPI()
    {
        WWWForm form = new WWWForm();
        form.AddField(FormFields.playerId, Matching.playerId);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("get_score"), form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (int.TryParse(www.downloadHandler.text, out int score))
                {
                    highScore = score;
                }
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                yield return GetScoreAPI();
            }
        }
    }
}