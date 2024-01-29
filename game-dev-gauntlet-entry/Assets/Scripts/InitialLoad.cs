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
    
    private VideoRender _videoRender;
    
    private void Start()
    {
        Application.runInBackground = true;
        Time.timeScale = 1.0f;
        
        // Referencing the Scripts from GameObjects
        _videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();

        skipButton.SetActive(false);
        loadingScreen.SetActive(false);
        StartCoroutine(FirstTimeCheck(mainScene));
    }

    private IEnumerator FirstTimeCheck(string scene)
    {
        // Checks if First Time Playing
        int firstTimePlaying = PlayerPrefs.GetInt("FirstTimePlaying", 1);
        if (firstTimePlaying == 1)
        {
            // Play Animation Video of Game Intro 
            _videoRender.PlayIntro();
            yield return new WaitForSeconds(5);
            skipButton.SetActive(true);
        }
        else if (firstTimePlaying == 0)
        {
            StartCoroutine(LoadAsynchronously(scene));
        }

        yield return null;
    }

    public IEnumerator LoadAsynchronously(string scene)
    {
        // Loading Screen
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
