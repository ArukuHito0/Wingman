using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingLabel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI userNameText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image userIcon;
    [SerializeField] Image rankBox;
    [SerializeField] Image highlight;
    [SerializeField] Sprite[] rankBoxSprites;

    public void Initialize(int rank, int score, string name, Sprite icon, bool isMine = false)
    {
        if (rank == 0)
        {
            rankText.text = "-";
        }
        else
        {
            rankText.text = rank.ToString();

            if (rank <= 3)
            {
                rankBox.sprite = rankBoxSprites[rank - 1];
            }
        }

        if (score == 0)
        {
            scoreText.text = "----- -----";
        }
        else
        {
            scoreText.text = score.ToString();
        }

        userNameText.text = name;

        if (icon != null)
            userIcon.sprite = icon;

        highlight.enabled = isMine;
    }
}
