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
    public GameObject modePanel;
    public GameObject miscPanel;
    public GameObject settingsPanel;
    public GameObject loadingScreen;
    public Animator levelSelectionController;
    public Slider loadingSlider;
    public Sprite[] loadingBgSprite;
    public Image loadingBg;
    public GameObject loadingBgObj;
    public Text loadingProvinceText;
    public Image loadingFloat;
    public int floatDistance;
    private string[] firstTimeKeyName = {"FirstTimeAntique", "FirstTimeAklan", "FirstTimeCapiz", "FirstTimeNegrosOcc", "FirstTimeGuimaras", "FirstTimeIloilo"};
    public string mainScene = "MainScene";
    public string kitchenScene = "KitchenScene";
    public string restaurantScene = "RestaurantScene";
    public int levelId;
    private bool canPlayTravel = false;
    
    private PlayerLives playerLives;
    private PlayerProvince playerProvince;
    private UpdateDisplayMain updateDisplayMain;
    private VideoRender videoRender;

    private void Start()
    {
        Application.runInBackground = true;
        Time.timeScale = 1.0f;
        
        playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
        updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();
        
        if (SceneManager.GetActiveScene().name == mainScene) 
        {
            modePanel.SetActive(false);
            miscPanel.SetActive(false);
            settingsPanel.SetActive(false);
            
            videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
            PlayAnimation();
        }
            
        loadingScreen.SetActive(false);
    }

    public void SelectProvince(int selected)
    {
        levelId = selected;
        updateDisplayMain.UpdateDescription(levelId);

        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        if (provinceUnlocked != levelId && provinceUnlocked < levelId)
            playerProvince.ProvincePurchasing();
        else
        {
            updateDisplayMain.DisableProvince();
            levelSelectionController.SetTrigger("OpenSelection");
        }
    }

    public void UnselectProvince()
    {
        updateDisplayMain.EnableProvince();
        levelSelectionController.SetTrigger("CloseSelection");
    }
    
    public void LoadKitchen()
    {
        if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) > 0)
        {
            StartCoroutine(LoadAsynchronously(kitchenScene));
            PlayerPrefs.SetInt("ProvinceCurrent", levelId);
            levelSelection.SetActive(false);
            loadingBgObj.SetActive(true);
        }
    }

    public void LoadRestaurant()
    {
        StartCoroutine(LoadAsynchronously(restaurantScene));
        PlayerPrefs.SetInt("ProvinceCurrent", levelId);
        levelSelection.SetActive(false);
        loadingBgObj.SetActive(true);
    }

    public void LoadBack(string scene)
    {
        PlayerPrefs.SetInt("FailsBeforeSuccess", 0);
        StartCoroutine(LoadAsynchronously(scene));
    }

    public void LoadFinishBack(string scene)
    {
        if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) > 0)
        {
            StartCoroutine(LoadAsynchronously(scene));
            canPlayTravel = true;
        }
    }

    private IEnumerator LoadAsynchronously(string scene)
    {
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
        if (canPlayTravel) PlayAnimation();  
    }

    public void PlayAnimation()
    {
        canPlayTravel = false;
        if (videoScreen) videoScreen.SetActive(true);

        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        if (PlayerPrefs.GetInt(firstTimeKeyName[provinceUnlocked - 1], 1) == 1)
        {
            levelSelection.SetActive(true);
            levelSelection.SetActive(false);
            videoRender.PlayTravel(provinceUnlocked);
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
