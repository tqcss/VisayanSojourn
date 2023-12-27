using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TravelNegrosOcc : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 4 && PlayerPrefs.GetInt("FirstTimeNegrosOcc", 1) == 1)
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
