using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager instance;

    // AudioSource
    public AudioSource bgmSource;

    public AudioSource seSource;

    // ===== BGM =====
    public AudioClip titleBGM;

    public AudioClip puzzleBGM;

    public AudioClip shootingBGM;

    // ===== SE =====
    public AudioClip ClickSE;

    public AudioClip ContactSE;

    public AudioClip evolutionSE;

    public AudioClip spawnGravitySE;

    public AudioClip bigBangSE;

    public AudioClip explosionSE;

    public AudioClip shotSE;

    public AudioClip gravityChargedSE;

    public AudioClip shotHitSE;

    public AudioClip planetBreakSE;

    // 現在再生中BGM
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

        // 音量読み込み
        LoadVolume();

        Debug.Log("AudioManager 初期化");
    }

    // =========================
    // BGM音量変更
    // =========================
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;

        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    // =========================
    // SE音量変更
    // =========================
    public void SetSEVolume(float volume)
    {
        seSource.volume = volume;

        PlayerPrefs.SetFloat("SEVolume", volume);
    }

    // =========================
    // 音量読み込み
    // =========================
    private void LoadVolume()
    {
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);

        float seVolume = PlayerPrefs.GetFloat("SEVolume", 1f);

        bgmSource.volume = bgmVolume;

        seSource.volume = seVolume;
    }

    // =========================
    // BGM再生
    // =========================
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
            case "Title":
                clip = titleBGM;
                break;

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

            bgmSource.loop = true;

            bgmSource.Play();

            Debug.Log("BGM再生 : " + bgmName);
        }
    }

    // =========================
    // SE再生
    // =========================
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

            case "Evo":
                clip = evolutionSE;
                break;

            case "UseGravity":
                clip = spawnGravitySE;
                break;

            case "BigBang":
                clip = bigBangSE;
                break;

            case "Explosion":
                clip = explosionSE;
                break;

            case "Shot":
                clip = shotSE;
                break;

            case "GravityCharged":
                clip = gravityChargedSE;
                break;

            case "Hit":
                clip = shotHitSE;
                break;

            case "Broken":
                clip = planetBreakSE;
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