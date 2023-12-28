using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoRender : MonoBehaviour
{
    public VideoClip[] videoClip;
    public VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }
    
    public void PlayIntro()
    {
        videoPlayer.clip = videoClip[0];
        if (videoPlayer) videoPlayer.Play();
    }

    public void PlayScroll()
    {
        videoPlayer.clip = videoClip[0];
        if (videoPlayer) videoPlayer.Play();
    }

    public void PlayTravel(int provinceUnlocked)
    {
        videoPlayer.clip = videoClip[provinceUnlocked - 1];
        if (videoPlayer) videoPlayer.Play();
    }
}
