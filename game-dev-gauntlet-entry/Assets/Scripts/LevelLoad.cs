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
    public int floatDistance;
    public string[] firstTimeKeyName = {"FirstTimeAntique", "FirstTimeAklan", "FirstTimeCapiz", "FirstTimeNegrosOcc", "FirstTimeGuimaras", "FirstTimeIloilo"};
    public string mainScene = "MainScene";
    public string kitchenScene = "KitchenScene";
    public string restaurantScene = "RestaurantScene";
    public int levelId;
    private bool canPlayAnimation = false;
    
    private AudioManager _audioManager;
    private PlayerLives _playerLives;
    private PlayerProvince _playerProvince;
    private UpdateDisplayMain _updateDisplayMain;
    private VideoRender _videoRender;

    private void Start()
    {
        Application.runInBackground = true;
        Time.timeScale = 1.0f;
        
        // Referencing the Scripts from GameObjects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        _playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
        _updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();
        
        if (SceneManager.GetActiveScene().name == mainScene) 
        {
            _videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
            modePanel.SetActive(false);
            miscPanel.SetActive(false);
            settingsPanel.SetActive(false);
            
            PlayAnimation();
            _audioManager.PlayBackgroundMusic(_audioManager.mainMusic);
        }
            
        loadingScreen.SetActive(false);
    }

    public void SelectProvince(int selected)
    {
        // Execute if Province is Selected
        levelId = selected;
        _updateDisplayMain.UpdateDescription(levelId);

        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        if (provinceUnlocked != levelId && provinceUnlocked < levelId)
        {
            // Execute if the Selected Province is Locked
            _playerProvince.ProvincePurchasing();
        }
        else
        {
            if (levelId <= provinceUnlocked)
            {
                // Execute if the Selected Province is Unlocked
                _updateDisplayMain.DisableProvince();
                levelSelectionController.SetTrigger("OpenSelection");
                _audioManager.PlayThemeMusic(_audioManager.provinceThemeMusic[levelId - 1], levelId);
            }
        }
    }

    public void UnselectProvince()
    {
        // Execute if Province is Deselected
        _updateDisplayMain.EnableProvince();
        levelSelectionController.SetTrigger("CloseSelection");
        _audioManager.PlayBackgroundMusic(_audioManager.mainMusic);
    }
    
    public void LoadKitchen()
    {
        // Prepare Load for Kitchen Scene
        if (PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax) > 0)
        {
            StartCoroutine(LoadAsynchronously(kitchenScene));
            PlayerPrefs.SetInt("ProvinceCurrent", levelId);
            levelSelection.SetActive(false);
            loadingBgObj.SetActive(true);
        }
    }

    public void LoadRestaurant()
    {
        // Prepare Load for Restaurant Scene
        StartCoroutine(LoadAsynchronously(restaurantScene));
        PlayerPrefs.SetInt("ProvinceCurrent", levelId);
        levelSelection.SetActive(false);
        loadingBgObj.SetActive(true);
    }

    public void LoadBack(string scene)
    {
        // Prepare Load to go Back to Main
        PlayerPrefs.SetInt("FailsBeforeSuccess", 0);
        StartCoroutine(LoadAsynchronously(scene));
    }

    public void LoadFinishBack(string scene)
    {
        // Prepare Load to go Back to Main
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
        
        // Loading Screen
        loadingScreen.SetActive(true);
        loadingProvinceText.text = loadingBgSprite[levelId - 1].name.Replace("image_", "").Replace("_", " ").ToUpper();
        loadingBg.sprite = loadingBgSprite[levelId - 1];
        loadingSlider.value = 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        
        float progress = 0;
        double firstFloatPosX = loadingFloat.GetComponent<RectTransform>().localPosition.x;

        while (!operation.isDone)
        {
            progress = Mathf.MoveTowards(progress, Mathf.Clamp01(operation.progress / 0.9f), Time.deltaTime / 3.14f);
            loadingSlider.value = progress;
            
            double moveDishPosX = firstFloatPosX + (progress * floatDistance);
            loadingFloat.GetComponent<RectTransform>().localPosition = new Vector3((int)moveDishPosX, loadingFloat.GetComponent<RectTransform>().localPosition.y, 0);

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

    public void PlayAnimation()
    {
        // Play Animation Video of Traveling from One to the Next
        canPlayAnimation = false;
        if (videoScreen) videoScreen.SetActive(true);

        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        if (PlayerPrefs.GetInt(firstTimeKeyName[provinceUnlocked - 1], 1) == 1)
        {
            levelSelection.SetActive(false);
            _videoRender.PlayTravel(provinceUnlocked);
            PlayerPrefs.SetInt(firstTimeKeyName[provinceUnlocked - 1], 0);
        }
        else
        {
            AfterTravel();
        }
    }

    public void AfterTravel()
    {
        if (videoScreen) videoScreen.SetActive(false);
        levelSelection.SetActive(true);

        if (SceneManager.GetActiveScene().name == mainScene) 
        {
            modePanel.SetActive(true);
            miscPanel.SetActive(true);
            settingsPanel.SetActive(true);
        }
    }
}
