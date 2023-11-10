using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoInitial : MonoBehaviour
{
    
    void Start()
    {
        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            PlayVideo();
        }
    }

    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer)
        {
            //videoPlayer.Play();
        }
    }

}
