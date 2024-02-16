using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateDisplayMain : MonoBehaviour
{
    // PlayerCoins
    public Text coinsText;
    public Text coinBagText;

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
    private PlayerCoins _playerCoins;
    private PlayerLives _playerLives;
    private PlayerProvince _playerProvince;
    
    private void Awake()
    {
        // Reference the scripts from game objects
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        _playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        _playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
    }

    public void UpdateDisplayCoins()
    {
        // Update coin display on the main scene
        if (SceneManager.GetActiveScene().name == _levelLoad.mainScene)
            coinsText.text = string.Format("{0:0.00}", PlayerPrefs.GetFloat("GlobalCoins", _playerCoins.initialCoins));
    }

    public void UpdateDisplayCoinBag()
    {
        // Update coin bag display on the main scene
        if (SceneManager.GetActiveScene().name == _levelLoad.mainScene)
            coinBagText.text = string.Format("{0:0.00}", PlayerPrefs.GetFloat("CoinsGenerated", 0));
    }

    public void UpdateDisplayLives()
    {
        // Update lives display on the main scene
        if (SceneManager.GetActiveScene().name == _levelLoad.mainScene)
        {
            int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
            float lifeCooldown = PlayerPrefs.GetFloat("LifeCooldown", _playerLives.lifeMaxCooldown);
            
            if (globalLives < _playerLives.livesMax)
            {
                if (_playerLives.inCooldown)
                {
                    // Display the life cooldown timer as 00:00
                    int minutes = Mathf.FloorToInt(lifeCooldown / 60);
                    int seconds = Mathf.FloorToInt(lifeCooldown % 60);
                    livesCooldownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                }
            } 
            else if (globalLives == _playerLives.livesMax)
            {
                // Display "full" if the player has maximum life
                livesCooldownText.text = "FULL";
            }
            
            livesText.text = globalLives.ToString();
            livesImage.sprite = heartSprite[globalLives];
        }
    }

    public void UpdateDisplayProvince()
    {
        // Update province display on the main scene
        if (SceneManager.GetActiveScene().name == _levelLoad.mainScene)
        {
            int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
            int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);

            for (int i = 0; i < _playerProvince.provinceTotal; i++)
            {
                // Display provinces that are unlocked with location marker
                bool isProvinceUnlocked = i < provinceUnlocked;
                locationMarker[i].sprite = (isProvinceUnlocked) ? unlockLocation : null;
                locationButton[i].interactable = (isProvinceUnlocked) ? enableInteract : false;
                locationButtonObj[i].SetActive((isProvinceUnlocked) ? true : false);
                provinceCostObj[i].SetActive(false);
            }
            
            if (provinceCompleted == provinceUnlocked && provinceUnlocked < _playerProvince.provinceTotal)
            {
                // Display the province that is currently locked with lock sprite and the province cost
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
        // Disable location marker interaction if it is pressed
        backDescButton.interactable = true;
        enableInteract = false;
        for (int j = 0; j < _playerProvince.provinceTotal; j++)
            locationButton[j].interactable = false;
    }

    public void EnableProvince()
    {
        // Enable location marker interaction if back button is pressed
        backDescButton.interactable = false;
        StartCoroutine(DelayEnable());
    }

    private IEnumerator DelayEnable()
    {
        // Delay enable the location marker interaction (to remove the bug)
        yield return new WaitForSeconds(1);
        for (int j = 0; j < PlayerPrefs.GetInt("ProvinceUnlocked", 1); j++)
            if (j < _playerProvince.provinceTotal)
                locationButton[j].interactable = true;
        enableInteract = true;
        UpdateDisplayProvince();
    }

    public void UpdateDescription(int levelId)
    {
        // Update the description panel based on the selected province
        provinceDesc.sprite = descSprite[levelId - 1];

        // Enable play kitchen button if the player global life is more than 0
        int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
        playKitchenButton.SetActive((globalLives <= _playerLives.livesMax && globalLives > 0) ? true : false);
        unplayKitchenButton.SetActive((globalLives <= _playerLives.livesMax && globalLives > 0) ? false : true);

        // Enable play restaurant button if the player completed the kitchen mode of a selected province
        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        playRestaurantButton.SetActive((levelId <= provinceCompleted) ? true : false);
        unplayRestaurantButton.SetActive((levelId <= provinceCompleted) ? false : true);
    }
}
