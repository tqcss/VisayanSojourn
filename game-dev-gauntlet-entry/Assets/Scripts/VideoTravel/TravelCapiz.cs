using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TravelCapiz : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 3 && PlayerPrefs.GetInt("FirstTimeCapiz", 1) == 1)
        {
            PlayVideo();
        }
    }

    public void PlayVideo()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer)
        {
            videoPlayer.Play();
        }
    }

}
