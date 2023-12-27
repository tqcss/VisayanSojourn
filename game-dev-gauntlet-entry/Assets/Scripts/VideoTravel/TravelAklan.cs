using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TravelAklan : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 2 && PlayerPrefs.GetInt("FirstTimeAklan", 1) == 1)
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
