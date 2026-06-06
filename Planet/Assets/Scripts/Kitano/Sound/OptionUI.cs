using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Slider bgmSlider;

    public Slider seSlider;

    private EventTrigger seSliderEventTrigger;

    void Start()
    {
        // 保存済み音量を反映
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);

        seSlider.value = PlayerPrefs.GetFloat("SEVolume", 1f);

        // スライダー変更時イベント
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);

        seSlider.onValueChanged.AddListener(SetSEVolume);

        seSliderEventTrigger = seSlider.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => AudioManager.instance.PlayTestSound());
        seSliderEventTrigger.triggers.Add(entry);
    }

    public void SetBGMVolume(float volume)
    {
        AudioManager.instance.SetBGMVolume(volume);
    }

    public void SetSEVolume(float volume)
    {
        AudioManager.instance.SetSEVolume(volume);
    }
}