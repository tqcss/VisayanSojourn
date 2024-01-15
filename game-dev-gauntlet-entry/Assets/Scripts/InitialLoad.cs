using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class InitialLoad : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public GameObject skipButton;
    public string mainScene = "MainScene";
    public string introScene = "IntroScene";
    private VideoRender videoRender;
    
    private void Start()
    {
        #if UNITY_ANDROID
            Screen.SetResolution(360, 640, true);
        #endif
        
        videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
        Application.runInBackground = true;

        skipButton.SetActive(false);
        loadingScreen.SetActive(false);
        StartCoroutine(FirstTimeCheck(mainScene));
    }

    private IEnumerator FirstTimeCheck(string scene)
    {
        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            videoRender.PlayIntro();
            yield return new WaitForSeconds(2);
            skipButton.SetActive(true);
        }
        else if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 0)
            StartCoroutine(LoadAsynchronously(scene));
        
        yield return null;
    }

    public IEnumerator LoadAsynchronously(string scene)
    {
        PlayerPrefs.SetInt("FirstTimePlaying", 0);
        skipButton.SetActive(false);
        loadingScreen.SetActive(true);
        loadingSlider.value = 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        
        float progress = 0;

        while (!operation.isDone)
        {
            progress = Mathf.MoveTowards(progress, Mathf.Clamp01(operation.progress / 0.9f), Time.deltaTime / 3.14f);
            loadingSlider.value = progress;
            
            if (progress >= 1f)
            {
                yield return new WaitForSeconds(1);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingScreen.SetActive(false);
    }
}
