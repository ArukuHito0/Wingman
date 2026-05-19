using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;

    public int maxScore = 30000;

    public TextMeshProUGUI scoreText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    // 加点
    public void AddScore(int value)
    {
        score += value;

        // 最大制限
        if (score > maxScore)
        {
            score = maxScore;
        }

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

    void UpdateUI()
    {
        scoreText.text = "Score : " + score;
    }
}