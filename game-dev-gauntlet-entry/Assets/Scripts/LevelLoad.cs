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

    private int numberLevel;
    private string sceneLevel = "KitchenR6";
    private bool canPlayAnimation = false;
    public int antiqueAnimSec, aklanAnimSec, capizAnimSec, negrosOccAnimSec, guimarasAnimSec, iloiloAnimSec;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainScene") StartCoroutine(PlayAnimation());
        loadingScreen.SetActive(false);
    }

    public void SelectProvince (int selected)
    {
        numberLevel = selected - 1;
        UpdateDescription(numberLevel);
    }
    
    public void LoadLevel()
    {
        if (!(PlayerPrefs.GetInt("GlobalLives", 3) <= 0))
        {
            StartCoroutine(LoadAsynchronously(sceneLevel));
            PlayerPrefs.SetInt("ProvinceCurrent", (numberLevel + 1));
            loadingBgObj.SetActive(true);
        }
    }

    public void LoadBack (string scene)
    {
        Debug.Log("Go to Main");
        PlayerPrefs.SetInt("FailsBeforeWin", 0);
        StartCoroutine(LoadAsynchronously(scene));
    }

    public void LoadFinishBack (string scene)
    {
        if (PlayerPrefs.GetInt("GlobalLives", 3) > 0 && PlayerPrefs.GetInt("ProceedNext", 0) == 1)
        {
            StartCoroutine(LoadAsynchronously(scene));
            canPlayAnimation = true;
            PlayerPrefs.SetInt("ProceedNext", 0);
        }
    }

    private IEnumerator LoadAsynchronously (string scene)
    {
        loadingScreen.SetActive(true);
        loadingSlider.value = 0;
        loadingProvinceText.text = loadingBgSprite[numberLevel].name.Replace("image_", "").Replace("_", " ").ToUpper();

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

        if (SceneManager.GetActiveScene().name == "MainScene") loadingScreen.SetActive(false);
        if (canPlayAnimation == true) StartCoroutine(PlayAnimation());  
    }

    private IEnumerator PlayAnimation()
    {
        canPlayAnimation = false;
        if (videoScreen) videoScreen.SetActive(true);

        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 1 && PlayerPrefs.GetInt("FirstTimeAntique", 1) == 1)
        {
            levelSelection.SetActive(false);
            yield return new WaitForSeconds(antiqueAnimSec);
            PlayerPrefs.SetInt("FirstTimeAntique", 0); 
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 2 && PlayerPrefs.GetInt("FirstTimeAklan", 1) == 1)
        {
            levelSelection.SetActive(false);
            yield return new WaitForSeconds(aklanAnimSec);
            PlayerPrefs.SetInt("FirstTimeAklan", 0); 
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 3 && PlayerPrefs.GetInt("FirstTimeCapiz", 1) == 1)
        {
            levelSelection.SetActive(false);
            yield return new WaitForSeconds(capizAnimSec);
            PlayerPrefs.SetInt("FirstTimeCapiz", 0); 
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 4 && PlayerPrefs.GetInt("FirstTimeNegrosOcc", 1) == 1)
        {
            levelSelection.SetActive(false);
            yield return new WaitForSeconds(negrosOccAnimSec);
            PlayerPrefs.SetInt("FirstTimeNegrosOcc", 0); 
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 5 && PlayerPrefs.GetInt("FirstTimeGuimaras", 1) == 1)
        {
            levelSelection.SetActive(false);
            yield return new WaitForSeconds(guimarasAnimSec);
            PlayerPrefs.SetInt("FirstTimeGuimaras", 0); 
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 6 && PlayerPrefs.GetInt("FirstTimeIloilo", 1) == 1)
        {
            levelSelection.SetActive(false);
            yield return new WaitForSeconds(iloiloAnimSec);
            PlayerPrefs.SetInt("FirstTimeIloilo", 0); 
        }

        if (videoScreen) videoScreen.SetActive(false);
        levelSelection.SetActive(true);
        yield return null;
    }

    public void UpdateDescription (int numberLevel)
    {
        provinceDesc.sprite = descSprite[numberLevel];
        loadingBg.sprite = loadingBgSprite[numberLevel];
        
        if (numberLevel < 6)
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
