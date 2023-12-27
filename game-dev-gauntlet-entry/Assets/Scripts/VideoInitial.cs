using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoInitial : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            PlayVideo();
        }
    }

    public void PlayVideo()
    {
        if (videoPlayer)
        {
            videoPlayer.Play();
        }
    }

}
