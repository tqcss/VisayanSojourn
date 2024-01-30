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
        // Set the application as running in background, and the time scale should be in normal state
        Application.runInBackground = true;
        Time.timeScale = 1.0f;
        
        // Reference the scripts from game objects
        _videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();

        // Set the game objects
        skipButton.SetActive(false);
        loadingScreen.SetActive(false);
        StartCoroutine(FirstTimeCheck(mainScene));
    }

    private IEnumerator FirstTimeCheck(string scene)
    {
        // Check if the player is playing for the first time
        int firstTimePlaying = PlayerPrefs.GetInt("FirstTimePlaying", 1);
        if (firstTimePlaying == 1)
        {
            // Play the video of the game intro
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
        PlayerPrefs.SetInt("FirstTimePlaying", 0);
        skipButton.SetActive(false);
        loadingScreen.SetActive(true);
        loadingSlider.value = 0;

        // Load asynchronously to the selected scene, with loading screen active
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        
        float progress = 0;

        // Increase the loading progress if the async operation is not yet done
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
