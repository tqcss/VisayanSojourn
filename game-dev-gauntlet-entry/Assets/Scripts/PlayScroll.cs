using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayScroll : MonoBehaviour
{
    void Awake()
    {
        PlayVideo();
    }

    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer)
        {
            videoPlayer.frame = 0;
            videoPlayer.Play();
        }
    }
}
