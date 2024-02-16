using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoad : MonoBehaviour
{
    public GameObject videoScreen;
    public GameObject levelSelection;
    public Animator levelSelectionController;
    
    public GameObject modePanel;
    public GameObject miscPanel;
    public GameObject settingsPanel;
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public Sprite[] loadingBgSprite;
    public Image loadingBg;
    public GameObject loadingBgObj;
    public Text loadingProvinceText;
    public Image loadingFloat;
    public int floatDistanceApart;

    // To check if it is the first time or not the player played the travel animation
    public string[] primalTravelKeyNames = {"PrimalTravelAntique", "PrimalTravelAklan", "PrimalTravelCapiz", "PrimalTravelNegrosOcc", "PrimalTravelGuimaras", "PrimalTravelIloilo", ""};
    // To check if it is the first time or not the player played the selected level
    public string[] initialPlayedKeyNames = {"InitialPlayedAntique", "InitialPlayedAklan", "InitialPlayedCapiz", "InitialPlayedNegrosOcc", "InitialPlayedGuimaras", "InitialPlayedIloilo", ""};
    
    public string introScene = "IntroScene";
    public string mainScene = "MainScene";
    public string kitchenScene = "KitchenScene";
    public string restaurantScene = "RestaurantScene";
    public string travelScene = "TravelScene"; 
    public int levelId;
    private bool canPlayAnimation = false;
    
    private AudioManager _audioManager;
    private PlayerLives _playerLives;
    private PlayerProvince _playerProvince;
    private UpdateDisplayMain _updateDisplayMain;

    private void Start()
    {
        // Set the application as running in background, and the time scale should be in normal state
        Application.runInBackground = true;
        Time.timeScale = 1.0f;
        
        // Reference the scripts from game objects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        _playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
        _updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();
        
        // Set the game objects
        if (SceneManager.GetActiveScene().name == mainScene) 
        {
            modePanel.SetActive(true);
            miscPanel.SetActive(true);
            settingsPanel.SetActive(true);
            _updateDisplayMain.UpdateDisplayProvince();
            
            PlayAnimation();
            StartCoroutine(WaitForMusic());
        }
            
        loadingScreen.SetActive(false);
    }

    public void SelectProvince(int selected)
    {
        // Execute if the province is selected
        levelId = selected;
        _updateDisplayMain.UpdateDescription(levelId);

        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        if (provinceUnlocked != levelId && provinceUnlocked < levelId)
        {
            // Execute if the selected province is locked
            _playerProvince.ProvincePurchasing();
        }
        else
        {
            if (levelId <= provinceUnlocked)
            {
                // Execute if the selected province is unlocked
                _updateDisplayMain.DisableProvince();
                levelSelectionController.SetTrigger("OpenSelection");
                _audioManager.PlayThemeMusic(_audioManager.provinceThemeMusic[levelId - 1], true, levelId);
            }
        }
    }

    public void UnselectProvince()
    {
        // Execute if the province is deselected
        _updateDisplayMain.EnableProvince();
        levelSelectionController.SetTrigger("CloseSelection");
        _audioManager.PlayBackgroundMusic(_audioManager.mainMusic, true);
    }
    
    public void LoadKitchen()
    {
        // Prepare load the kitchen mode if the player global life is more than 0
        if (PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax) > 0)
        {
            StartCoroutine(LoadAsynchronously(kitchenScene));
            PlayerPrefs.SetInt("ProvinceCurrent", levelId);
            
            levelSelection.SetActive(false);
            loadingBgObj.SetActive(true);

            if (PlayerPrefs.GetInt(initialPlayedKeyNames[levelId - 1], 0) == 0)
                PlayerPrefs.SetInt(initialPlayedKeyNames[levelId - 1], 1);
        }
    }

    public void LoadRestaurant()
    {
        // Prepare load the restaurant mode
        StartCoroutine(LoadAsynchronously(restaurantScene));
        PlayerPrefs.SetInt("ProvinceCurrent", levelId);

        levelSelection.SetActive(false);
        loadingBgObj.SetActive(true);
    }

    public void LoadIntro()
    {
        // Load the intro scene
        _audioManager.StopMusic();
        SceneManager.LoadScene(introScene);
    }

    public void LoadBack(string scene)
    {
        // Prepare load to go back to the main menu
        PlayerPrefs.SetInt("FailsBeforeSuccess", 0);
        StartCoroutine(LoadAsynchronously(scene));
    }

    public void LoadFinishBack(string scene)
    {
        // Prepare load to go back to the main menu
        if (PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax) > 0)
        {
            StartCoroutine(LoadAsynchronously(scene));
            canPlayAnimation = true;
        }
    }

    private IEnumerator LoadAsynchronously(string scene)
    {
        _audioManager.StopMusic();
        _audioManager.startSfx.Play();
        
        loadingScreen.SetActive(true);
        loadingProvinceText.text = loadingBgSprite[levelId - 1].name.Replace("image_", "").Replace("_", " ").ToUpper();
        loadingBg.sprite = loadingBgSprite[levelId - 1];
        loadingSlider.value = 0;

        // Load asynchronously to the selected scene, with loading screen active
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        
        float progress = 0;
        float firstFloatPosX = loadingFloat.GetComponent<RectTransform>().localPosition.x;
        float firstFloatPosY = loadingFloat.GetComponent<RectTransform>().localPosition.y;

        // Increase the loading progress if the async operation is not yet done
        while (!operation.isDone)
        {
            progress = Mathf.MoveTowards(progress, Mathf.Clamp01(operation.progress / 0.9f), Time.deltaTime / 3.14f);
            loadingSlider.value = progress;
            
            // Set the position of the image float of loading screen based on the progress
            float moveFloatPosX = firstFloatPosX + (progress * floatDistanceApart);
            loadingFloat.GetComponent<RectTransform>().localPosition = new Vector2((int)moveFloatPosX, firstFloatPosY);

            // Go to the prompt scene if the progress reaches 100%
            if (progress >= 1f)
            {
                yield return new WaitForSeconds(1);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (SceneManager.GetActiveScene().name == mainScene) loadingScreen.SetActive(false);
        if (canPlayAnimation) PlayAnimation();  
    }

    public IEnumerator WaitForMusic()
    {
        yield return new WaitForSeconds(1);
        _audioManager.PlayBackgroundMusic(_audioManager.mainMusic, true);
    }

    public void PlayAnimation()
    {
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);

        // Check if the player is first time unlocking the province
        if (PlayerPrefs.GetInt(primalTravelKeyNames[provinceUnlocked - 1], 1) == 1)
        {
            // Play the video of traveling from a current province to the next one
            SceneManager.LoadScene(travelScene);
        }
    }

    public int CheckModeId()
    {
        /*
            Mode
            1: Kitchen Mode
            2: Restaurant Mode
        */
        
        // Check the current mode and return the mode id
        int modeId = 0;
        switch (SceneManager.GetActiveScene().name)
        {
            case "KitchenScene":
                modeId = 1;
                break;
            case "RestaurantScene":
                modeId = 2;
                break;
        }
        return modeId;
    }
}
