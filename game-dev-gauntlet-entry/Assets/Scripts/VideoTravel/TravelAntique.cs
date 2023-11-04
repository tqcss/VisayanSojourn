using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TravelAntique : MonoBehaviour
{

    void Awake()
    {
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 1 && PlayerPrefs.GetInt("FirstTimeAntique", 1) == 1)
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
