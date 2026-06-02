using UnityEngine;
using UnityEngine.Video;

public class PlayWebGLVideo : MonoBehaviour
{
    //  インスペクターから動画ファイル名を自由に変えられるようにします
    [Header("動画ファイル名（拡張子まで記入）")]
    [SerializeField] private string videoFileName = "help_move.mp4";

    void Start()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            // WebGL・エディタ両対応のURLパス設定
            videoPlayer.source = VideoSource.Url;

            string basePath = Application.streamingAssetsPath;
            videoPlayer.url = System.IO.Path.Combine(basePath, videoFileName).Replace("\\", "/");

            // 消音対策と再生準備
            videoPlayer.Prepare();
            videoPlayer.prepareCompleted += (source) => source.Play();
        }
        else
        {
            Debug.LogError($"{gameObject.name} に VideoPlayer が見つかりません！");
        }
    }
}
