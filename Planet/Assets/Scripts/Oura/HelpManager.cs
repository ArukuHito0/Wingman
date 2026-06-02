using UnityEngine;

public class HelpManager : MonoBehaviour
{
    public GameObject[] panels;

    private int currentIndex = 0;

    void Start()
    {
        ShowPanel(0);
    }

    // 次へ
    public void NextPage()
    {
        currentIndex++;

        if (currentIndex >= panels.Length)
        {
            currentIndex = 0;
        }

        ShowPanel(currentIndex);
    }

    // 戻る
    public void PrevPage()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = panels.Length - 1;
        }

        ShowPanel(currentIndex);
    }

    void ShowPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }
    }
}