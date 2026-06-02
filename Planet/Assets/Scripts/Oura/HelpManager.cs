using UnityEngine;
using UnityEngine.UI;

public class HelpPageManager : MonoBehaviour
{
    [Header("ヘルプ画面の親オブジェクト（全体を消す用）")]
    [SerializeField] private GameObject helpWindow;

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
        // ボタンに機能を登録
        if (nextButton != null) nextButton.onClick.AddListener(NextPage);
        if (prevButton != null) prevButton.onClick.AddListener(PrevPage);

        if (openHelpButton != null) openHelpButton.onClick.AddListener(OpenHelp);
        if (closeHelpButton != null) closeHelpButton.onClick.AddListener(CloseHelp);

        //  ゲーム開始時はヘルプ画面全体を非表示にする
        if (helpWindow != null) helpWindow.SetActive(false);

        //  戻るボタンも、最初はゲーム画面に出ないように非表示にしておく
        if (prevButton != null) prevButton.gameObject.SetActive(false);
    }

    //  ヘルプ画面を開く処理
    public void OpenHelp()
    {
        if (helpWindow != null)
        {
            // 1. ヘルプ画面全体を表示する
            helpWindow.SetActive(true);

            // 2. 1ページ目（0番目）を表示し、ボタンの状態を正しくセットする
            ShowPage(0);
        }
    }

    //  ヘルプ画面を閉じる処理
    public void CloseHelp()
    {
        if (helpWindow != null)
        {
            helpWindow.SetActive(false);
        }

        //  閉じたときは、戻るボタンも一緒に非表示にする
        if (prevButton != null) prevButton.gameObject.SetActive(false);
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

        //  1ページ目（indexが0）なら「戻るボタン」は非表示、2ページ目以降なら表示！
        if (prevButton != null) prevButton.gameObject.SetActive(currentPageIndex > 0);

        //  最終ページなら「次へボタン」は非表示、それ以外なら表示！
        if (nextButton != null) nextButton.gameObject.SetActive(currentPageIndex < panels.Length - 1);
    }

    public void NextPage() => ShowPage(currentPageIndex + 1);
    public void PrevPage() => ShowPage(currentPageIndex - 1);
}
