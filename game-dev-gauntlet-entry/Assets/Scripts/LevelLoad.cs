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
    public TextMeshProUGUI ProvinceText;

    public GameObject LoadingAntique;
    public GameObject LoadingAklan;
    public GameObject LoadingCapiz;
    public GameObject LoadingNegrosOcc;
    public GameObject LoadingGuimaras;
    public GameObject LoadingIloilo;

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
        StartCoroutine(LoadAsynchronously(sceneLevel));
    }

    public void LoadFinishBack (string sceneLevel)
    {
        if (!(PlayerPrefs.GetInt("GlobalLives", 3) <= 0))
        {
            StartCoroutine(LoadAsynchronously(sceneLevel));
            canPlayAnimation = true;
        }
    }

    IEnumerator LoadAsynchronously (string sceneLevel)
    {
        LoadingScreen.SetActive(true);
        LoadingSlider.value = 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneLevel);
        operation.allowSceneActivation = false;
        
        float progress = 0;
        yield return new WaitForSeconds(1);

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
            LoadingAntique.SetActive(true);
        } 
        else if (sceneLevel == "AklanScene") 
        {
            LoadingAklan.SetActive(true);
        }
        else if (sceneLevel == "CapizScene")
        {
            LoadingCapiz.SetActive(true);
        }
        else if (sceneLevel == "NegrosOccScene")
        {
            LoadingNegrosOcc.SetActive(true);
        }
        else if (sceneLevel == "GuimarasScene")
        {
            LoadingGuimaras.SetActive(true);
        }
        else if (sceneLevel == "IloiloScene")
        {
            LoadingIloilo.SetActive(true);
        }
    }

    public void UpdateDescription (string sceneLevel)
    {
        if (sceneLevel == "AntiqueScene") 
        {
            ProvinceText.text = "ANTIQUE";
        } 
        else if (sceneLevel == "AklanScene") 
        {
            ProvinceText.text = "AKLAN";
        }
        else if (sceneLevel == "CapizScene")
        {
            ProvinceText.text = "CAPIZ";
        }
        else if (sceneLevel == "NegrosOccScene")
        {
            ProvinceText.text = "NEGROS OCCIDENTAL";
        }
        else if (sceneLevel == "GuimarasScene")
        {
            ProvinceText.text = "GUIMARAS";
        }
        else if (sceneLevel == "IloiloScene")
        {
            ProvinceText.text = "ILOILO";
        } 
    }

    public void DoQuit()
    {
        Application.Quit();
    }
}
