using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI newRecordText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button titleButton;

    [SerializeField] private int[] textRotationAngles;
    [SerializeField] private float animationTime;

    private void Start()
    {
        CursorManager.Instance.SetCursorTexture(CursorManager.CursorType.Default);

        playButton.interactable = false;
        titleButton.interactable = false;

        StartCoroutine(ScoreVisualizeAnimation());

        var highScore = ScoreManager.Instance.GetHighScore();
        highScoreText.text = highScore > 0 ? highScore.ToString() : "---------";

        if (ScoreManager.Instance.GetScore() > ScoreManager.Instance.GetHighScore())
        {
            StartCoroutine(ScoreManager.Instance.SendScoreAPI());
        }
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void Replay()
    {

        SceneManager.LoadScene("Shooting Phase");

    }

    private IEnumerator ScoreVisualizeAnimation()
    {
        int score = ScoreManager.Instance.GetScore();
        int highScore = ScoreManager.Instance.GetHighScore();
        float scoreforStr = 0;
        float time = 0f;
        bool isPlayed = false;

        TextMeshProUGUI effect = scoreText.transform.Find("ScoreTextEffect").GetComponent<TextMeshProUGUI>();
        effect.text = score.ToString();

        RectTransform rectTransform = scoreText.GetComponent<RectTransform>();
        Vector3 rotation = new Vector3(0, 0, textRotationAngles[Random.Range(0, textRotationAngles.Length)]);
        rectTransform.rotation = Quaternion.Euler(rotation);

        while (time < animationTime)
        {
            time += Time.deltaTime;

            scoreforStr = Mathf.Lerp(0, score, time / animationTime);
            scoreText.text = scoreforStr.ToString("F0");

            if (time / animationTime >= 0.7f && !isPlayed)
            {
                AudioManager.instance.PlaySE("ResultScore");
                isPlayed = true;
            }

            yield return null;
        }

        if (score > highScore)
        {
            newRecordText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        playButton.interactable = true;
        titleButton.interactable = true;

        yield return new WaitForSeconds(0.75f);

        AudioManager.instance.PlayBGM("Result");
    }
}
