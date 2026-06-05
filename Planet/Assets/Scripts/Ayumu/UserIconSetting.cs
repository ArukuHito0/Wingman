using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public struct IconData
{
    public string iconName;
    public Sprite iconSprite;
}

public class UserIconSetting : MonoBehaviour
{
    public static UserIconSetting Instance { get; private set; }

    public List<IconData> iconList;

    [SerializeField]
    private GameObject iconContainerPrefab;

    [SerializeField]
    private Image myIcon;

    [SerializeField]
    private Transform viewContent;

    [SerializeField]
    private GameObject iconScrollView;

    [SerializeField]
    private TextMeshProUGUI iconNameText;

    private bool isOpend = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(GetUserIconAPI());
        CreateIocnContainers();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void OpenOrCloseIconInventory()
    {
        isOpend = !iconScrollView.active;
        iconScrollView.SetActive(isOpend);
    }

    private void CreateIocnContainers()
    {
        for (int i = 0; i < iconList.Count; i++)
        {
            CreateIconContainer(iconList[i], i);
        }
    }

    private void CreateIconContainer(IconData data, int num)
    {
        UserIconContainer uic = Instantiate(iconContainerPrefab, viewContent).GetComponent<UserIconContainer>();
        uic.Initialize(data, num, iconNameText);
    }

    public void SetMyIcon(int idx)
    {
        myIcon.sprite = iconList[idx].iconSprite;
    }

    public IEnumerator GetUserIconAPI()
    {
        WWWForm form = new WWWForm();
        form.AddField(FormFields.playerId, Matching.playerId);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("get_user_icon"), form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (int.TryParse(www.downloadHandler.text, out int idx))
                {
                    myIcon.sprite = iconList[idx].iconSprite;
                    Debug.Log(idx);
                }
            }
            else
            {
                Debug.Log("アイコンをDBにセットできませんでした");
            }
        }
    }

    public IEnumerator SetUserIconAPI(int idx)
    {
        WWWForm form = new WWWForm();
        form.AddField(FormFields.playerId, Matching.playerId);
        form.AddField(FormFields.userIcon, idx);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("set_user_icon"), form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("アイコンをDBにセットしました");
            }
            else
            {
                Debug.Log("アイコンをDBにセットできませんでした");
            }
        }
    }
}
