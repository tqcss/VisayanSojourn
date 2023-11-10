using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoad : MonoBehaviour
{

    public GameObject VideoScreen;
    public GameObject MainMenu;
    public GameObject LoadingScreen;
    public Slider LoadingSlider;

    public GameObject LoadingAntique;
    public GameObject LoadingAklan;
    public GameObject LoadingCapiz;
    public GameObject LoadingNegrosOcc;
    public GameObject LoadingGuimaras;
    public GameObject LoadingIloilo;
    public GameObject DescAntique;
    public GameObject DescAklan;
    public GameObject DescCapiz;
    public GameObject DescNegrosOcc;
    public GameObject DescGuimaras;
    public GameObject DescIloilo;
    public GameObject PlayButton;
    public GameObject NotAvailText;

    private string sceneLevel;
    private bool canPlayAnimation = false;
    public int AntiqueAnimSec, AklanAnimSec, CapizAnimSec, NegrosOccAnimSec, GuimarasAnimSec, IloiloAnimSec;

    private void Start()
    {
        StartCoroutine(PlayAnimation());
    }

    public void SelectProvince (string selected)
    {
        sceneLevel = selected;
        UpdateDescription(sceneLevel);
    }
    
    public void LoadLevel (bool playLevel)
    {
        if (sceneLevel != null)
        {
            if (!(PlayerPrefs.GetInt("GlobalLives", 3) <= 0))
            {
                StartCoroutine(LoadAsynchronously(sceneLevel));
                LoadBackgroundScreen(sceneLevel);
            }
        }
    }

    public void LoadBack (string sceneLevel)
    {
        Debug.Log("Go to Main");
        PlayerPrefs.SetInt("FailsBeforeWin", 0);
        StartCoroutine(LoadAsynchronously(sceneLevel));
    }

    public void LoadFinishBack (string sceneLevel)
    {
        if (PlayerPrefs.GetInt("GlobalLives", 3) > 0 && PlayerPrefs.GetInt("ProceedNextProvince", 0) == 1)
        {
            StartCoroutine(LoadAsynchronously(sceneLevel));
            canPlayAnimation = true;
            PlayerPrefs.SetInt("ProceedNextProvince", 0);
        }
    }

    IEnumerator LoadAsynchronously (string sceneLevel)
    {
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
        if (canPlayAnimation == true)
        {
            StartCoroutine(PlayAnimation());
        }   
    }

    private IEnumerator PlayAnimation()
    {
        canPlayAnimation = false;
        VideoScreen.SetActive(true);

        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 1 && PlayerPrefs.GetInt("FirstTimeAntique", 1) == 1)
        {
            //Debug.Log("Play Start -> Antique Animation");
            MainMenu.SetActive(false);
            yield return new WaitForSeconds(AntiqueAnimSec);
            PlayerPrefs.SetInt("FirstTimeAntique", 0); 
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 2 && PlayerPrefs.GetInt("FirstTimeAklan", 1) == 1)
        {
            //Debug.Log("Play Antique -> Aklan Animation");
            MainMenu.SetActive(false);
            yield return new WaitForSeconds(AklanAnimSec);
            PlayerPrefs.SetInt("FirstTimeAklan", 0); 
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 3 && PlayerPrefs.GetInt("FirstTimeCapiz", 1) == 1)
        {
            //Debug.Log("Play Aklan -> Capiz Animation");
            MainMenu.SetActive(false);
            yield return new WaitForSeconds(CapizAnimSec);
            PlayerPrefs.SetInt("FirstTimeCapiz", 0); 
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 4 && PlayerPrefs.GetInt("FirstTimeNegrosOcc", 1) == 1)
        {
            //Debug.Log("Play Capiz -> Negros Occ Animation");
            MainMenu.SetActive(false);
            yield return new WaitForSeconds(NegrosOccAnimSec);
            PlayerPrefs.SetInt("FirstTimeNegrosOcc", 0); 
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 5 && PlayerPrefs.GetInt("FirstTimeGuimaras", 1) == 1)
        {
            //Debug.Log("Play Negros Occ -> Guimaras Animation");
            MainMenu.SetActive(false);
            yield return new WaitForSeconds(GuimarasAnimSec);
            PlayerPrefs.SetInt("FirstTimeGuimaras", 0); 
        }
        else if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 6 && PlayerPrefs.GetInt("FirstTimeIloilo", 1) == 1)
        {
            //Debug.Log("Play Guimaras -> Iloilo Animation");
            MainMenu.SetActive(false);
            yield return new WaitForSeconds(IloiloAnimSec);
            PlayerPrefs.SetInt("FirstTimeIloilo", 0); 
        }

        VideoScreen.SetActive(false);
        MainMenu.SetActive(true);
        yield return null;
    }

    public void LoadBackgroundScreen (string sceneLevel)
    {
        LoadingAntique.SetActive(false);
        LoadingAklan.SetActive(false);
        LoadingCapiz.SetActive(false);
        LoadingNegrosOcc.SetActive(false);
        LoadingGuimaras.SetActive(false);
        LoadingIloilo.SetActive(false);
        
        if (sceneLevel == "AntiqueScene") 
        {
            PlayerPrefs.SetInt("ProvinceCurrent", 1);
            LoadingAntique.SetActive(true);
        } 
        else if (sceneLevel == "AklanScene") 
        {
            PlayerPrefs.SetInt("ProvinceCurrent", 2);
            LoadingAklan.SetActive(true);
        }
        else if (sceneLevel == "CapizScene")
        {
            PlayerPrefs.SetInt("ProvinceCurrent", 3);
            LoadingCapiz.SetActive(true);
        }
        else if (sceneLevel == "NegrosOccScene")
        {
            PlayerPrefs.SetInt("ProvinceCurrent", 4);
            LoadingNegrosOcc.SetActive(true);
        }
        else if (sceneLevel == "GuimarasScene")
        {
            PlayerPrefs.SetInt("ProvinceCurrent", 5);
            LoadingGuimaras.SetActive(true);
        }
        else if (sceneLevel == "IloiloScene")
        {
            PlayerPrefs.SetInt("ProvinceCurrent", 6);
            LoadingIloilo.SetActive(true);
        }
    }

    public void UpdateDescription (string sceneLevel)
    {
        DescAntique.SetActive(false);
        DescAklan.SetActive(false);
        DescCapiz.SetActive(false);
        DescNegrosOcc.SetActive(false);
        DescGuimaras.SetActive(false);
        DescIloilo.SetActive(false);
        if (sceneLevel == "AntiqueScene") 
        {
            DescAntique.SetActive(true);
            PlayButton.SetActive(true);
            NotAvailText.SetActive(false);
        } 
        else if (sceneLevel == "AklanScene") 
        {
            DescAklan.SetActive(true);
            PlayButton.SetActive(true);
            NotAvailText.SetActive(false);
        }
        else if (sceneLevel == "CapizScene")
        {
            DescCapiz.SetActive(true);
            PlayButton.SetActive(false);
            NotAvailText.SetActive(true);
        }
        else if (sceneLevel == "NegrosOccScene")
        {
            DescNegrosOcc.SetActive(true);
            PlayButton.SetActive(false);
            NotAvailText.SetActive(true);
        }
        else if (sceneLevel == "GuimarasScene")
        {
            DescGuimaras.SetActive(true);
            PlayButton.SetActive(false);
            NotAvailText.SetActive(true);
        }
        else if (sceneLevel == "IloiloScene")
        {
            DescIloilo.SetActive(true);
            PlayButton.SetActive(false);
            NotAvailText.SetActive(true);
        } 
    }

    public void DoQuit()
    {
        Application.Quit();
    }
}
