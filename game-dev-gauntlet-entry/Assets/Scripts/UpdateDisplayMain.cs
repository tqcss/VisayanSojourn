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
    private bool enableInteract = true;

    // ModePanel
    public Sprite[] descSprite;
    public Image provinceDesc;
    public GameObject playKitchenButton;
    public GameObject playRestaurantButton;
    public GameObject unplayKitchenButton;
    public GameObject unplayRestaurantButton;

    private LevelLoad _levelLoad;
    private PlayerLives _playerLives;
    private PlayerProvince _playerProvince;
    
    private void Awake()
    {
        // Referencing the Scripts from GameObjects
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        _playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
    }

    public void UpdateDisplayCoins()
    {
        // Update Coin Display on the Main Scene
        if (SceneManager.GetActiveScene().name == _levelLoad.mainScene)
            coinsText.text = string.Format("{0:0.00}", PlayerPrefs.GetFloat("GlobalCoins", 0));
    }

    public void UpdateDisplayLives()
    {
        // Update Lives Display on the Main Scene
        if (SceneManager.GetActiveScene().name == _levelLoad.mainScene)
        {
            int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
            float lifeCooldown = PlayerPrefs.GetFloat("LifeCooldown", _playerLives.lifeMaxCooldown);
            
            if (globalLives < _playerLives.livesMax)
            {
                if (_playerLives.inCooldown)
                {
                    int minutes = Mathf.FloorToInt(lifeCooldown / 60);
                    int seconds = Mathf.FloorToInt(lifeCooldown % 60);
                    livesCooldownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                }
            } 
            else if (globalLives == _playerLives.livesMax)
            {
                livesCooldownText.text = "FULL";
            }
            
            livesText.text = globalLives.ToString();
            livesImage.sprite = heartSprite[globalLives];
        }
    }

    public void UpdateDisplayProvince()
    {
        // Update Province Display on the Main Scene
        if (SceneManager.GetActiveScene().name == _levelLoad.mainScene)
        {
            int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
            int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);

            for (int i = 0; i < _playerProvince.provinceTotal; i++)
            {
                bool isProvinceUnlocked = i < provinceUnlocked;
                locationMarker[i].sprite = (isProvinceUnlocked) ? unlockLocation : null;
                locationButton[i].interactable = (isProvinceUnlocked) ? enableInteract : false;
                locationButtonObj[i].SetActive((isProvinceUnlocked) ? true : false);
                provinceCostObj[i].SetActive(false);
            }
            
            if (provinceCompleted == provinceUnlocked && provinceUnlocked < _playerProvince.provinceTotal)
            {
                locationButtonObj[provinceCompleted].SetActive(true);
                locationButton[provinceCompleted].interactable = true;
                locationMarker[provinceCompleted].sprite = lockLocation;
                provinceCostObj[provinceCompleted].SetActive(true);
                provinceCostText[provinceCompleted].text = _playerProvince.provinceCost[provinceCompleted].ToString();
            }
            
            mapImage.sprite = mapSprite[provinceUnlocked - 1];
        }
    }

    public void DisableProvince()
    {
        // Disable Location Marker Interaction if it is pressed
        backDescButton.interactable = true;
        enableInteract = false;
        for (int j = 0; j < _playerProvince.provinceTotal; j++)
            locationButton[j].interactable = false;
    }

    public void EnableProvince()
    {
        // Disable Location Marker Interaction if backButton is pressed
        backDescButton.interactable = false;
        StartCoroutine(DelayEnable());
    }

    private IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(1);
        for (int j = 0; j < PlayerPrefs.GetInt("ProvinceUnlocked", 1); j++)
            if (j < _playerProvince.provinceTotal)
                locationButton[j].interactable = true;
        enableInteract = true;
    }

    public void UpdateDescription(int levelId)
    {
        // Update Description Interface based on Selected Province
        provinceDesc.sprite = descSprite[levelId - 1];

        int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
        playKitchenButton.SetActive((globalLives <= _playerLives.livesMax && globalLives > 0) ? true : false);
        unplayKitchenButton.SetActive((globalLives <= _playerLives.livesMax && globalLives > 0) ? false : true);

        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        playRestaurantButton.SetActive((levelId <= provinceCompleted) ? true : false);
        unplayRestaurantButton.SetActive((levelId <= provinceCompleted) ? false : true);
    }
}
