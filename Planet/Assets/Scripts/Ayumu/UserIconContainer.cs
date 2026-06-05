using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserIconContainer : MonoBehaviour
{
    [SerializeField]
    private Image iconImage;

    private int iconNumber;

    public void Initialize(Sprite icon, int num)
    {
        iconImage.sprite = icon;
        iconNumber = num;
    }

    public void SetUserIcon()
    {
        StartCoroutine(UserIconSetting.Instance.SetUserIconAPI(iconNumber));
        UserIconSetting.Instance.SetMyIcon(iconNumber);
    }
}
