using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;

public class InitialLoad : MonoBehaviour
{
    public GameObject introTrailerScreen;
    public GameObject titleScreen;
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public GameObject skipButton;
    public GameObject playButton;
    public GameObject quitButton;
    public string mainScene = "MainScene";
    public string travelScene = "TravelScene";
    public Text versionText;
    public GameObject versionTextObj;
    public string version;
    
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
        titleScreen.SetActive(false);
        loadingScreen.SetActive(false);
        StartCoroutine(FirstTimeCheck());
    }

    private IEnumerator FirstTimeCheck()
    {
        // Check if the player is playing for the first time
        int firstTimePlaying = PlayerPrefs.GetInt("FirstTimePlaying", 1);
        
        // Play the intro depending on the first time status
        StartCoroutine(_videoRender.PlayIntro(firstTimePlaying));
        yield return new WaitForSeconds(3);
        skipButton.SetActive(true);

        yield return null;
    }

    public IEnumerator DisplayTitleScreen()
    {
        // Display the title screen if the player is not the first time playing the game
        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            PlayerPrefs.SetInt("FirstTimePlaying", 0);
            StartCoroutine(_videoRender.PlayIntro(0));
            skipButton.SetActive(false);
        }
        else
        {
            // Set the game objects
            skipButton.SetActive(false);
            introTrailerScreen.SetActive(false);
            titleScreen.SetActive(true);

            playButton.SetActive(false);
            quitButton.SetActive(false);
            versionTextObj.SetActive(false);

            yield return new WaitForSeconds(3);
            playButton.SetActive(true);
            quitButton.SetActive(true);
            versionTextObj.SetActive(true);
            versionText.text = "VERSION " + version;
        }
    }

    public void LoadMain()
    {
        // Execute if the play button is pressed
        titleScreen.SetActive(false);
        StartCoroutine(LoadAsynchronously(travelScene));
    }

    public IEnumerator LoadAsynchronously(string scene)
    {
        skipButton.SetActive(false);
        introTrailerScreen.SetActive(false);
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
            
            // Go to the prompt scene if the progress reaches 100%
            if (progress >= 1f)
            {
                yield return new WaitForSeconds(1);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        loadingScreen.SetActive(false);
    }

    public void DoQuit()
    {
        // Quit the application if the quit button is pressed
        Application.Quit();
    }
}
