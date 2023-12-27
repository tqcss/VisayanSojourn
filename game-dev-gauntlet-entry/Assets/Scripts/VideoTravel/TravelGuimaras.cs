using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TravelGuimaras : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Prepare();
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 5 && PlayerPrefs.GetInt("FirstTimeGuimaras", 1) == 1)
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
