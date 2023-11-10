using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayScroll : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        PlayVideo();
    }

    public void PlayVideo()
    {
        if (videoPlayer)
        {
            videoPlayer.frame = 1;
            videoPlayer.Play();
        }
    }
}
