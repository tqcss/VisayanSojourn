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
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public Sprite[] loadingBgSprite;
    public Image loadingBg;
    public GameObject loadingBgObj;
    public Text loadingProvinceText;
    public Image loadingDish;
    public int dishDistance;
    public Sprite[] descSprite;
    public Image provinceDesc;
    public GameObject playButton;
    public GameObject notPlayButton;
    public int[] animationSeconds;
    private string[] firstTime = {"FirstTimeAntique", "FirstTimeAklan", "FirstTimeCapiz", "FirstTimeNegrosOcc", "FirstTimeGuimaras", "FirstTimeIloilo"};
    public string mainScene = "MainScene";
    public string kitchenScene = "KitchenR6";
    public int levelId;
    private bool canPlayTravel = false;
    
    private PlayerLives playerLives;
    private PlayerProvince playerProvince;
    private VideoRender videoRender;

    private void Start()
    {
        playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        playerProvince = GameObject.FindGameObjectWithTag("mainScript").GetComponent<PlayerProvince>();
        Application.runInBackground = true;
        
        if (SceneManager.GetActiveScene().name == mainScene) 
        {
            videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
            StartCoroutine(PlayAnimation());
        }
            
        loadingScreen.SetActive(false);
    }

    public void SelectProvince(int selected)
    {
        levelId = selected - 1;
        UpdateDescription(levelId);
        playerProvince.DisableProvince();
    }
    
    public void LoadLevel()
    {
        if (PlayerPrefs.GetInt("GlobalLives", 3) > 0)
        {
            StartCoroutine(LoadAsynchronously(kitchenScene));
            PlayerPrefs.SetInt("ProvinceCurrent", (levelId + 1));
            loadingBgObj.SetActive(true);
        }
    }

    public void LoadBack(string scene)
    {
        PlayerPrefs.SetInt("FailsBeforeWin", 0);
        StartCoroutine(LoadAsynchronously(scene));
    }

    public void LoadFinishBack(string scene)
    {
        if (PlayerPrefs.GetInt("GlobalLives", 3) > 0)
        {
            StartCoroutine(LoadAsynchronously(scene));
            canPlayTravel = true;
        }
    }

    private IEnumerator LoadAsynchronously(string scene)
    {
        loadingScreen.SetActive(true);
        loadingProvinceText.text = loadingBgSprite[levelId].name.Replace("image_", "").Replace("_", " ").ToUpper();
        loadingBg.sprite = loadingBgSprite[levelId];
        loadingSlider.value = 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        
        float progress = 0;
        double firstDishPosX = loadingDish.GetComponent<RectTransform>().localPosition.x;

        while (!operation.isDone)
        {
            progress = Mathf.MoveTowards(progress, Mathf.Clamp01(operation.progress / 0.9f), Time.deltaTime / 3.14f);
            loadingSlider.value = progress;
            
            double moveDishPosX = firstDishPosX + (progress * dishDistance);
            loadingDish.GetComponent<RectTransform>().localPosition = new Vector3((int)moveDishPosX, loadingDish.GetComponent<RectTransform>().localPosition.y, 0);

            if (progress >= 1f)
            {
                yield return new WaitForSeconds(1);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (SceneManager.GetActiveScene().name == mainScene) 
            loadingScreen.SetActive(false);
        if (canPlayTravel) 
            StartCoroutine(PlayAnimation());  
    }

    private IEnumerator PlayAnimation()
    {
        canPlayTravel = false;
        if (videoScreen) 
            videoScreen.SetActive(true);

        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        if (PlayerPrefs.GetInt(firstTime[provinceUnlocked - 1], 1) == 1)
        {
            levelSelection.SetActive(false);
            videoRender.PlayTravel(provinceUnlocked);
            yield return new WaitForSeconds(animationSeconds[provinceUnlocked - 1]);
            PlayerPrefs.SetInt(firstTime[provinceUnlocked - 1], 0);
        }

        if (videoScreen) 
            videoScreen.SetActive(false);

        levelSelection.SetActive(true);
        yield return null;
    }

    public void UpdateDescription(int levelId)
    {
        provinceDesc.sprite = descSprite[levelId];
        
        if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) <= playerLives.livesTotal &&
            PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) > 0)
        {
            playButton.SetActive(true);
            notPlayButton.SetActive(false);
        }
        else
        {
            playButton.SetActive(false);
            notPlayButton.SetActive(true);
        }
    }

    public void DoQuit()
    {
        Application.Quit();
    }
}
