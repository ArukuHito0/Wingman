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
        rankText.text = rank.ToString();
        userNameText.text = name;
        scoreText.text = score.ToString();

        if (icon != null)
            userIcon.sprite = icon;

        highlight.enabled = isMine;

        if (rank <= 3)
        {
            rankBox.sprite = rankBoxSprites[rank - 1];
        }

    }
}
