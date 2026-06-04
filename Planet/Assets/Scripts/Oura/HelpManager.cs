using UnityEngine;
using UnityEngine.UI;

public class HelpPageManager : MonoBehaviour
{
    [Header("ヘルプ画面の親オブジェクト（全体を消す用）")]
    [SerializeField] private GameObject helpWindow;

    [Header(" ヘルプ開帳時に【非表示】にしたい他のキャンバスやUI")]
    [SerializeField] private GameObject[] otherUIObjects;

    [Header("ヘルプ画面を開く/閉じるボタン")]
    [SerializeField] private Button openHelpButton;
    [SerializeField] private Button closeHelpButton;

    [Header("ヘルプ用のパネル（GameObject）を順番に入れる")]
    [SerializeField] private GameObject[] panels;

    [Header("操作用ボタン")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    private int currentPageIndex = 0;

    void Start()
    {
        if (nextButton != null) nextButton.onClick.AddListener(NextPage);
        if (prevButton != null) prevButton.onClick.AddListener(PrevPage);

        if (openHelpButton != null) openHelpButton.onClick.AddListener(OpenHelp);
        if (closeHelpButton != null) closeHelpButton.onClick.AddListener(CloseHelp);

        if (helpWindow != null) helpWindow.SetActive(false);
        if (prevButton != null) prevButton.gameObject.SetActive(false);
    }

    //  ヘルプ画面を開く処理
    public void OpenHelp()
    {
        if (helpWindow != null)
        {
            helpWindow.SetActive(true);
            ShowPage(0);

            // 他のキャンバスやUIをすべて非表示にする
            ToggleOtherUI(false);
        }
    }

    //  ヘルプ画面を閉じる処理
    public void CloseHelp()
    {
        if (helpWindow != null)
        {
            helpWindow.SetActive(false);
        }
        if (prevButton != null) prevButton.gameObject.SetActive(false);

        // ヘルプを閉じたら、他のUIを元通り表示する
        ToggleOtherUI(true);
    }

    //  他のUIの一括表示・非表示を切り替える関数
    private void ToggleOtherUI(bool isActive)
    {
        if (otherUIObjects == null) return;

        foreach (var ui in otherUIObjects)
        {
            if (ui != null)
            {
                ui.SetActive(isActive);
            }
        }
    }

    public void ShowPage(int index)
    {
        if (panels == null || panels.Length == 0) return;

        currentPageIndex = Mathf.Clamp(index, 0, panels.Length - 1);

        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null)
            {
                panels[i].SetActive(i == currentPageIndex);
            }
        }

        if (prevButton != null) prevButton.gameObject.SetActive(currentPageIndex > 0);
        if (nextButton != null) nextButton.gameObject.SetActive(currentPageIndex < panels.Length - 1);
    }

    public void NextPage() => ShowPage(currentPageIndex + 1);
    public void PrevPage() => ShowPage(currentPageIndex - 1);
}
