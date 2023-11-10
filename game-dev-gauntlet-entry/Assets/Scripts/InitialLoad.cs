using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class InitialLoad : MonoBehaviour
{

    public int TrailerTime;
    public GameObject LoadingScreen;
    public Slider LoadingSlider;
    
    private string sceneLevel = "MainScene";
    private void Start()
    {
        StartCoroutine(FirstTimeCheck(sceneLevel));
    }

    private IEnumerator FirstTimeCheck(string sceneLevel)
    {
        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            yield return new WaitForSeconds(TrailerTime);
            StartCoroutine(LoadAsynchronously(sceneLevel));
        }
        else if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 0)
        {
            StartCoroutine(LoadAsynchronously(sceneLevel));
        }
    }

    IEnumerator LoadAsynchronously (string sceneLevel)
    {

        PlayerPrefs.SetInt("FirstTimePlaying", 0);
        LoadingScreen.SetActive(true);
        LoadingSlider.value = 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLevel);
        operation.allowSceneActivation = false;
        
        float progress = 0;

        while (!operation.isDone)
        {
            progress = Mathf.MoveTowards(progress, Mathf.Clamp01(operation.progress / 0.9f), Time.deltaTime / 3.14f);
            LoadingSlider.value = progress;
            
            if (progress >= 1f)
            {
                yield return new WaitForSeconds(1);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        LoadingScreen.SetActive(false);

    }

}
