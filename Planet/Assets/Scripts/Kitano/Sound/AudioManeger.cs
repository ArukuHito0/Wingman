using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager instance;

    // AudioSource
    public AudioSource bgmSource;

    public AudioSource seSource;

    // BGM
    public AudioClip puzzleBGM;

    public AudioClip shootingBGM;
    // SE
    public AudioClip ClickSE;

    public AudioClip ContactSE;
    private string currentBGM = "";

    void Awake()
    {
        // 重複防止
        if (instance != null)
        {
            Destroy(gameObject);

            return;
        }

        instance = this;

        // シーン移動で破壊しない
        DontDestroyOnLoad(gameObject);

        Debug.Log("AudioManager 初期化");
    }

    public void PlayBGM(string bgmName)
    {
        // 同じBGMなら再生しない
        if (currentBGM == bgmName)
        {
            return;
        }

        currentBGM = bgmName;

        AudioClip clip = null;

        // BGM選択
        switch (bgmName)
        {
            case "Puzzle":
                clip = puzzleBGM;
                break;

            case "Shooting":
                clip = shootingBGM;
                break;
        }

        // 再生
        if (clip != null)
        {
            bgmSource.clip = clip;

            bgmSource.Play();

            Debug.Log("BGM再生 : " + bgmName);
        }

    }
    public void PlaySE(string seName)
    {
        AudioClip clip = null;

        // SE選択
        switch (seName)
        {
            case "Click":
                clip = ClickSE;
                break;

            case "Contact":
                clip = ContactSE;
                break;
        }

        // 再生
        if (clip != null)
        {
            seSource.PlayOneShot(clip);

            Debug.Log("SE再生 : " + seName);
        }
    }
}