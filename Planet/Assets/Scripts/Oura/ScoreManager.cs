using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("シーソー（回転）の設定")]
    [Tooltip("揺れる最大の角度")]
    public float maxRotationAngle = 20f;
    [Tooltip("揺れるスピード")]
    public float rotationSpeed = 15f;

    [Header("拡縮（スケール）の設定")]
    [Tooltip("基準となる基本サイズ")]
    public Vector3 baseScale = Vector3.one;
    [Tooltip("拡大縮小の振り幅（0.2なら基本サイズ±0.2）")]
    public float scaleAmplitude = 0.2f;
    [Tooltip("拡縮のスピード")]
    public float scaleSpeed = 20f;
    public float scaleUpTime = 0.3f;
    public float scaleDownTime = 0.1f;

    private RectTransform scoreTextRectTransform;
    private Vector3 scoreTextBaseScale;
    private Quaternion scoreTextBaseRotate;

    private static int score { get; set; } = 0;
    private static int highScore {  get; set; }

    public TextMeshProUGUI scoreTitleText;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        Instance = this;
        scoreTextRectTransform = scoreText?.GetComponent<RectTransform>();
    }

    private void Start()
    {
        scoreTextBaseScale = scoreTextRectTransform.localScale;
        scoreTextBaseRotate = scoreTextRectTransform.localRotation;

        score = 0;
        scoreText.text = 0.ToString("F0");
    }

    // 加点
    public void AddScore(int value)
    {
        UpdateUI(value);
        score += value;
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
    }

    private void UpdateUI(int amount)
    {
        StartCoroutine(ScoreCountUpAnimaiton(amount));

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

    private IEnumerator ScoreCountUpAnimaiton(int amount)
    {
        // カウントダウンのテキストのアニメーション
        if (scoreTextRectTransform == null) yield break;

        float time = 0;
        int score = GetScore();
        int afterScore = GetScore() + amount;
        float scoretostr = score;

        while (scoretostr < afterScore)
        {
            float zRotation = Mathf.Sin(Time.time * rotationSpeed) * maxRotationAngle;
            scoreTextRectTransform.localRotation = Quaternion.Euler(0f, 0f, zRotation);

            float scaleOffset = Mathf.Cos(Time.time * scaleSpeed) * scaleAmplitude;
            Vector3 animatedScale = baseScale + new Vector3(scaleOffset, scaleOffset, 0f);

            scoreTextRectTransform.localScale = animatedScale;
            scoretostr = Mathf.Lerp(score, afterScore, time / scaleUpTime);
            scoreText.text = scoretostr.ToString("F0");

            time += Time.deltaTime;

            yield return null;
        }

        time = 0;

        Vector3 scale = (Vector3)scoreTextRectTransform?.localScale;
        Quaternion rotation = (Quaternion)scoreTextRectTransform?.localRotation;

        while (time < scaleDownTime)
        {
            scoreTextRectTransform.localScale = Vector3.Lerp(scale, scoreTextBaseScale, time / scaleDownTime);
            scoreTextRectTransform.localRotation = Quaternion.Lerp(rotation, scoreTextBaseRotate, time / scaleDownTime);

            time += Time.deltaTime;

            yield return null;
        }

        scoreText.color = GetScoreColor(afterScore);
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

    private Color GetScoreColor(int score)
    {
        string colorCode = string.Empty;

        if (score > 60000)
        {
            colorCode = "DE2978";
        }
        else if (score > 40000)
        {
            colorCode = "D2392B";
        }
        else if (score > 20000)
        {
            colorCode = "CF8423";
        }
        else
        {
            colorCode = "E2BA4A";
        }

        if (ColorUtility.TryParseHtmlString(colorCode, out Color color))
        {
            return color;
        }
        else
        {
            return Color.white;
        }
    }
}