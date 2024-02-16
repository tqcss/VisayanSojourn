using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoRender : MonoBehaviour
{
    public string[] videoFile;
    public VideoPlayer videoPlayer;
    public bool isVideoPlaying;

    private InitialLoad _initialLoad;
    private LevelLoad _levelLoad;
    private SettleKitchen _settleKitchen;
    private TravelManager _travelManager;

    private void Awake()
    {
        // Reference the scripts from game objects
        videoPlayer = GetComponent<VideoPlayer>();
        try
        {
            _initialLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<InitialLoad>();
            _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
            _settleKitchen = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleKitchen>();
            _travelManager = GameObject.FindGameObjectWithTag("mainScript").GetComponent<TravelManager>();
        }
        catch (UnityException)
        {
            Debug.Log("...");
        }
    }
    
    public IEnumerator PlayIntro(int firstTimePlaying)
    {
        #if UNITY_ANDROID
            Handheld.PlayFullScreenMovie(videoFile[firstTimePlaying], Color.black, FullScreenMovieControlMode.Hidden);
            yield return new WaitForSeconds(0.01f);
            EndVideo();
        #endif
        
        #if UNITY_STANDALONE_WIN
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.dataPath + "/StreamingAssets" + "/" + videoFile[firstTimePlaying];
            StartCoroutine(PlayVideoOnPC());
            yield return null;
        #endif
    }

    public IEnumerator PlayTravel(int provinceUnlocked)
    {
        #if UNITY_ANDROID
            Handheld.PlayFullScreenMovie(videoFile[provinceUnlocked - 1], Color.black, FullScreenMovieControlMode.Hidden);
            yield return new WaitForSeconds(0.01f);
            EndVideo();
        #endif
        
        #if UNITY_STANDALONE_WIN
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.dataPath + "/StreamingAssets" + "/" + videoFile[provinceUnlocked - 1];
            StartCoroutine(PlayVideoOnPC());
            yield return null;
        #endif
    }

    public IEnumerator PlayScroll()
    {
        #if UNITY_ANDROID
            Handheld.PlayFullScreenMovie(videoFile[0], Color.black, FullScreenMovieControlMode.Hidden);
            yield return new WaitForSeconds(0.01f);
            EndVideo();
        #endif
        
        #if UNITY_STANDALONE_WIN
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.dataPath + "/StreamingAssets" + "/" + videoFile[0];
            StartCoroutine(PlayVideoOnPC());
            yield return null;
        #endif
    }

    public IEnumerator PlayVideoOnPC()
    {
        // Set video player for PC_WINDOWS
        var audioSource = videoPlayer.GetComponent<AudioSource>();
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.controlledAudioTrackCount = 1;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        // Prepare the video player
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        // Play the video player
        videoPlayer.Play();
        while (videoPlayer.isPlaying)
            yield return null;
        
        EndVideo();
    }

    private void EndVideo()
    {
        // After PlayIntro()
        if (SceneManager.GetActiveScene().name == "IntroScene")
        {
            StartCoroutine(_initialLoad.DisplayTitleScreen());
        }
        // After PlayTravel()
        else if (SceneManager.GetActiveScene().name == "TravelScene")
        {
            _travelManager.GoBack();
        }
        // After PlayScroll()
        else if (SceneManager.GetActiveScene().name == "KitchenScene")
        {
            _settleKitchen.DisplayRecipe();
            _settleKitchen.recipeScroll.SetActive(true);
            _settleKitchen.scroll.SetActive(true);
        }
    }

    public void SkipVideo()
    {
        // Skip the video if the skip button is pressed
        if (SceneManager.GetActiveScene().name == "IntroScene")
        {
            StopCoroutine(PlayVideoOnPC());
            videoPlayer.Stop();
        }
        else if (SceneManager.GetActiveScene().name == "KitchenScene")
        {
            StopCoroutine(PlayVideoOnPC());
            videoPlayer.time = (long)(videoPlayer.frame);
        }
    }
}
