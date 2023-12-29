using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoRender : MonoBehaviour
{
    public string[] videoFile;
    public VideoPlayer videoPlayer;
    private InitialLoad initialLoad;
    private LevelLoad levelLoad;
    private SettleLevel settleLevel;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        try
        {
            initialLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<InitialLoad>();
            levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
            settleLevel = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleLevel>();
        }
        catch (UnityException)
        {
            Debug.Log("...");
        }
    }
    
    public void PlayIntro()
    {
        #if UNITY_ANDROID
            Handheld.PlayFullScreenMovie(videoFile[0], Color.black, FullScreenMovieControlMode.CancelOnInput);
        #endif
        
        #if UNITY_STANDALONE
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/" + videoFile[0];
            StartCoroutine(SetPlayVideo());
        #endif

        #if UNITY_EDITOR
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/" + videoFile[0];
            StartCoroutine(SetPlayVideo());
        #endif
    }

    public void PlayTravel(int provinceUnlocked)
    {
        #if UNITY_ANDROID
            Handheld.PlayFullScreenMovie(videoFile[provinceUnlocked - 1], Color.black, FullScreenMovieControlMode.CancelOnInput);
        #endif
        
        #if UNITY_STANDALONE
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/" + videoFile[provinceUnlocked - 1];
            StartCoroutine(SetPlayVideo());
        #endif

        #if UNITY_EDITOR
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/" + videoFile[provinceUnlocked - 1];
            StartCoroutine(SetPlayVideo());
        #endif
    }

    public void PlayScroll()
    {
        #if UNITY_ANDROID
            Handheld.PlayFullScreenMovie(videoFile[0], Color.black, FullScreenMovieControlMode.CancelOnInput);
        #endif
        
        #if UNITY_STANDALONE
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/" + videoFile[0];
            StartCoroutine(SetPlayVideo());
        #endif

        #if UNITY_EDITOR
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/" + videoFile[0];
            StartCoroutine(SetPlayVideo());
        #endif
    }

    public IEnumerator SetPlayVideo()
    {
        var audioSource = videoPlayer.GetComponent<AudioSource>();
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
        while (videoPlayer.isPlaying)
            yield return null;
        
        // After PlayIntro()
        if (SceneManager.GetActiveScene().name == "IntroScene")
            StartCoroutine(initialLoad.LoadAsynchronously(initialLoad.mainScene));
        // After PlayTravel()
        else if (SceneManager.GetActiveScene().name == "MainScene")
            levelLoad.AfterTravel();
        // After PlayScroll()
        else if (SceneManager.GetActiveScene().name == "KitchenScene")
            settleLevel.DisplayRecipe();
    }
}
