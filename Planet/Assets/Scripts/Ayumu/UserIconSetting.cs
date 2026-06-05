using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserIconSetting : MonoBehaviour
{
    public static UserIconSetting Instance { get; private set; }

    [SerializeField]
    public Sprite[] icons;

    [SerializeField]
    private GameObject iconContainerPrefab;

    [SerializeField]
    private Image myIcon;

    [SerializeField]
    private Transform viewContent;

    [SerializeField]
    private GameObject iconScrollView;

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
        isOpend = !isOpend;
        iconScrollView.SetActive(isOpend);
    }

    private void CreateIocnContainers()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            CreateIconContainer(icons[i], i);
        }
    }

    private void CreateIconContainer(Sprite iconSprite, int num)
    {
        UserIconContainer uic = Instantiate(iconContainerPrefab, viewContent).GetComponent<UserIconContainer>();
        uic.Initialize(iconSprite, num);
    }

    public void SetMyIcon(int idx)
    {
        myIcon.sprite = icons[idx];
    }

    public IEnumerator GetUserIconAPI()
    {
        WWWForm form = new WWWForm();
        form.AddField(FormFields.playerId, 2);

        using (UnityWebRequest www = UnityWebRequest.Post(FormFields.GetFormURL("get_user_icon"), form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                if (int.TryParse(www.downloadHandler.text, out int idx))
                {
                    myIcon.sprite = icons[idx];
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
        form.AddField(FormFields.playerId, 2);
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
