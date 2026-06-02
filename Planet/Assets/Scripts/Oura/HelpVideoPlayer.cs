using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class HelpVideoPlayer : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private string fileName = "help_move.mp4";

    private void Start()
    {
        string path = Path.Combine(
            Application.streamingAssetsPath,
            fileName);

        videoPlayer.url = path;
        videoPlayer.Play();
    }
}