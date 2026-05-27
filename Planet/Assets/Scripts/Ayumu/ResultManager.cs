using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI newRecordText;

    private void Start()
    {
        //highScoreText.text = ScoreManager.Instance.GetHighScore().ToString();
        scoreText.text = ScoreManager.Instance.GetScore().ToString();

        //if (ScoreManager.Instance.GetScore() > ScoreManager.Instance.GetHighScore())
        //{
        //    newRecordText.enabled = true;
        //}
        //else
        //{
        //    newRecordText.enabled = false;
        //}
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
