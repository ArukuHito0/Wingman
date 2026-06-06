using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserIconContainer : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;

    private TextMeshProUGUI iconNameText;
    private int iconNumber;
    private string iconName;

    public IconData iconData;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Initialize(IconData data, int num, TextMeshProUGUI nameText)
    {
        iconImage.sprite = data.iconSprite;
        iconName = data.iconName;
        iconNumber = num;
        iconNameText = nameText;
    }

    public void SetUserIcon()
    {
        iconNameText.text = iconName;
        StartCoroutine(UserIconSetting.Instance.SetUserIconAPI(iconNumber));
        UserIconSetting.Instance.SetMyIcon(iconNumber);
    }
}
