using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class InitialLoad : MonoBehaviour
{
    public int trailerTime;
    public GameObject loadingScreen;
    public Slider loadingSlider;
    private string sceneLevel = "MainScene";
    private VideoRender videoRender;
    
    private void Start()
    {
        videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
        Application.runInBackground = true;

        loadingScreen.SetActive(false);
        StartCoroutine(FirstTimeCheck(sceneLevel));
    }

    private IEnumerator FirstTimeCheck(string sceneLevel)
    {
        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            videoRender.PlayIntro();
            yield return new WaitForSeconds(trailerTime);
            StartCoroutine(LoadAsynchronously(sceneLevel));
        }
        else if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 0)
        {
            StartCoroutine(LoadAsynchronously(sceneLevel));
        }
    }

    private IEnumerator LoadAsynchronously (string sceneLevel)
    {

        PlayerPrefs.SetInt("FirstTimePlaying", 0);
        loadingScreen.SetActive(true);
        loadingSlider.value = 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLevel);
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
