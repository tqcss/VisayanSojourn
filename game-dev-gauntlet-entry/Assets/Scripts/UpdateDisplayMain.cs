using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateDisplayMain : MonoBehaviour
{
    // PlayerCoins
    public Text coinsText;

    // PlayerLives
    public Sprite[] heartSprite;
    public Image livesImage;
    public Text livesText;
    public Text livesCooldownText;

    // PlayerProvince
    public Image[] locationMarker;
    public Button[] locationButton;
    public GameObject[] locationButtonObj;
    public Image mapImage;
    public Sprite[] mapSprite;
    public GameObject[] provinceCostObj;
    public Text[] provinceCostText;
    public Sprite unlockLocation;
    public Sprite lockLocation;
    public Button backDescButton;

    // ModePanel
    public Sprite[] descSprite;
    public Image provinceDesc;
    public GameObject playKitchenButton;
    public GameObject playRestaurantButton;
    public GameObject unplayKitchenButton;
    public GameObject unplayRestaurantButton;

    private LevelLoad levelLoad;
    private PlayerCoins playerCoins;
    private PlayerLives playerLives;
    private PlayerProvince playerProvince;
    private void Awake()
    {
        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
    }

    public void UpdateDisplayCoins()
    {
        if (SceneManager.GetActiveScene().name == levelLoad.mainScene)
            coinsText.text = string.Format("{0:0.00}", PlayerPrefs.GetFloat("GlobalCoins", 0));
    }

    public void UpdateDisplayLives()
    {
        if (SceneManager.GetActiveScene().name == levelLoad.mainScene)
        {
            int globalLives = PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax);
            float lifeCooldown = PlayerPrefs.GetFloat("LifeCooldown", playerLives.lifeMaxCooldown);
            livesText.text = globalLives.ToString();
            if (globalLives < playerLives.livesMax)
            {
                if (playerLives.inCooldown)
                {
                    int minutes = Mathf.FloorToInt(lifeCooldown / 60);
                    int seconds = Mathf.FloorToInt(lifeCooldown % 60);
                    livesCooldownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                }
            } 
            else if (globalLives == playerLives.livesMax)
            {
                livesCooldownText.text = "FULL";
            }
            livesImage.sprite = heartSprite[globalLives];
        }
    }

    public void UpdateDisplayProvince()
    {
        if (SceneManager.GetActiveScene().name == levelLoad.mainScene)
        {
            int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
            int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);

            for (int i = 0; i < playerProvince.provinceTotal; i++)
            {
                locationButton[i].interactable = false;
                locationButtonObj[i].SetActive(false);
            }
            for (int i = 0; i < provinceUnlocked; i++)
            {
                if (i < playerProvince.provinceTotal)
                {
                    locationMarker[i].sprite = unlockLocation;
                    locationButton[i].interactable = true;
                    locationButtonObj[i].SetActive(true);
                    provinceCostObj[i].SetActive(false);
                }    
            }
            if (provinceCompleted == provinceUnlocked && provinceUnlocked < playerProvince.provinceTotal)
            {
                locationButtonObj[provinceCompleted].SetActive(true);
                locationButton[provinceCompleted].interactable = true;
                locationMarker[provinceCompleted].sprite = lockLocation;
                provinceCostObj[provinceCompleted].SetActive(true);
                provinceCostText[provinceCompleted].text = playerProvince.provinceCost[provinceCompleted].ToString();
            }
            mapImage.sprite = mapSprite[provinceUnlocked - 1];
        }
    }

    public void DisableProvince()
    {
        backDescButton.interactable = true;
        for (int j = 0; j < playerProvince.provinceTotal; j++)
            locationButton[j].interactable = false;
    }

    public void EnableProvince()
    {
        backDescButton.interactable = false;
        StartCoroutine(DelayEnable());
    }

    private IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(1);
        for (int j = 0; j < PlayerPrefs.GetInt("ProvinceUnlocked", 1); j++)
            if (j < playerProvince.provinceTotal)
                locationButton[j].interactable = true;
    }

    public void UpdateDescription(int levelId)
    {
        provinceDesc.sprite = descSprite[levelId - 1];

        int globalLives = PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax);
        playKitchenButton.SetActive((globalLives <= playerLives.livesMax && globalLives > 0) ? true : false);
        unplayKitchenButton.SetActive((globalLives <= playerLives.livesMax && globalLives > 0) ? false : true);

        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        playRestaurantButton.SetActive((levelId <= provinceCompleted) ? true : false);
        unplayRestaurantButton.SetActive((levelId <= provinceCompleted) ? false : true);
    }
}
