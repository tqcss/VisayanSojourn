using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TravelGuimaras : MonoBehaviour
{
    
    void Awake()
    {
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 5 && PlayerPrefs.GetInt("FirstTimeGuimaras", 1) == 1)
        {
            PlayVideo();
        }
    }

    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer)
        {
            videoPlayer.Play();
        }
    }

}
