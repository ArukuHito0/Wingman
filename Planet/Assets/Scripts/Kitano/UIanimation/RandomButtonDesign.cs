using UnityEngine;
using UnityEngine.UI;

public class RandomButtonDesign : MonoBehaviour
{
    [Header("ボタンImage")]
    public Image targetImage;
    private Image buttonImage;

    [Header("ランダム候補")]
    public Sprite[] randomSprites;

    [HideInInspector]
    public Sprite selectedSprite;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    void Start()
    {
        if (randomSprites.Length == 0) return;

        int index = Random.Range(0, randomSprites.Length);

        selectedSprite = randomSprites[index];

        targetImage.sprite = selectedSprite;
        buttonImage.sprite = selectedSprite;
    }
}