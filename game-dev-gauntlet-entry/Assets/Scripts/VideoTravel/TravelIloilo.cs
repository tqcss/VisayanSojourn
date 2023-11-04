using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TravelIloilo : MonoBehaviour
{
    
    void Awake()
    {
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 6 && PlayerPrefs.GetInt("FirstTimeIloilo", 1) == 1)
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
