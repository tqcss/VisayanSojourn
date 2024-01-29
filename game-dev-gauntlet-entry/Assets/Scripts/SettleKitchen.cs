using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettleKitchen : MonoBehaviour
{
    public GameObject kitchenUI;
    public GameObject roundFinishUI;
    public GameObject recipeScroll;
    public GameObject scroll;
    public GameObject skipButton;
    public GameObject backButton;
    public GameObject startButton;
    public GameObject dishNameTextObj;
    public GameObject recipeTextObj;
    public GameObject dishMonoImage;
    public Text dishNameText;
    public Text recipeText;
    public Text livesText;
    public Text messageText;
    public string[] endMessage;
    public GameObject descriptionPanel;
    public GameObject homeButton;
    public GameObject restartButton;
    public GameObject nextButton;
    public int maximumRound;
    public int currentRound;
    private bool successRound = false;
    
    private AudioManager _audioManager;
    private DishList _dishList;
    private LevelLoad _levelLoad;
    private OrderManager _orderManager;
    private PlayerLives _playerLives;
    private PlayerProvince _playerProvince;
    private VideoRender _videoRender;

    private void Awake()
    {
        // Referencing the Scripts from GameObjects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _dishList = GameObject.FindGameObjectWithTag("dishList").GetComponent<DishList>();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        _playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        _playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
        _videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
        
        currentRound = (CheckCurrentRound()) ? PlayerPrefs.GetInt(_playerProvince.recipeDoneKeyName[PlayerPrefs.GetInt("ProvinceCurrent", 0) - 1], 1) : 1;
        skipButton.SetActive(false);
        StartCoroutine(PlayAnimation(true));
    }

    private bool CheckCurrentRound()
    {
        // Checks if Not on the Previous Round as the Current Unlocked Province
        return PlayerPrefs.GetInt("ProvinceCurrent", 0) == PlayerPrefs.GetInt("ProvinceUnlocked", 1) &&
               PlayerPrefs.GetInt("ProvinceCurrent", 0) != PlayerPrefs.GetInt("ProvinceCompleted", 0);
    }

    private IEnumerator PlayAnimation(bool firstPlay)
    {
        recipeScroll.SetActive(true);
        scroll.SetActive(false);
        dishNameTextObj.SetActive(false);
        recipeTextObj.SetActive(false);
        dishMonoImage.SetActive(false);
        backButton.SetActive(false);
        startButton.SetActive(false);
        kitchenUI.SetActive(false);
        roundFinishUI.SetActive(false);
        _levelLoad.loadingScreen.SetActive(false);
        
        if (firstPlay) 
        {
            // Play Animation Video of Recipe Scroll
            _videoRender.PlayScroll();
            yield return new WaitForSeconds(2);
            skipButton.SetActive(true);
        }
        else
        {
            DisplayRecipe();
        }
        
        yield return null;
    }

    public void DisplayRecipe()
    {
        // Display the Dish and its Recipe
        _dishList.PromptOrder();
        skipButton.SetActive(false);
        dishNameTextObj.SetActive(true);
        recipeTextObj.SetActive(true);
        dishMonoImage.SetActive(true);
        backButton.SetActive(true);
        startButton.SetActive(true);
        
        dishNameText.text = _orderManager.currentOrderPrompt.name;
        string currentRecipeText = "Ingredients: \n";
        for (int i = 0; i < _orderManager.currentOrderPrompt.recipe.Count; i++)
        {
            currentRecipeText = currentRecipeText + "  • " + _orderManager.currentOrderPrompt.recipe[i].name.Replace("_", " ") + "\n";
            recipeText.text = currentRecipeText;
        }
    }

    public void StartRound()
    {
        // Start when startButton is Pressed
        _audioManager.PlayBackgroundMusic(_audioManager.kitchenMusic);
        _audioManager.startSfx.Play();

        recipeScroll.SetActive(false);
        kitchenUI.SetActive(true);
        livesText.text = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax).ToString();
        _orderManager.StartTimer();
    }

    public void EndRound(bool success)
    {
        // End when checkButton is Pressed
        _audioManager.StopMusic();
        
        roundFinishUI.SetActive(true);
        int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
        
        if (success)
        {
            successRound = true;
            if (globalLives > 0)
            {
                messageText.text = endMessage[0];
                nextButton.SetActive(true);
                restartButton.SetActive(false);
                descriptionPanel.SetActive(true);
                homeButton.SetActive((currentRound < maximumRound) ? true : false);

                _playerLives.RewardLife(PlayerPrefs.GetInt("FailsBeforeSuccess", 0), CheckCurrentRound());
            }
        }
        else
        {
            successRound = false;
            messageText.text = (globalLives > 0) ? endMessage[1] : endMessage[2];
            restartButton.SetActive((globalLives > 0) ? true : false);

            livesText.text = globalLives.ToString();
            nextButton.SetActive(false);
            homeButton.SetActive(true);
            descriptionPanel.SetActive(false);
        }
    }

    public void OntoNextRound()
    {
        int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        currentRound++;
        if (CheckCurrentRound()) 
            PlayerPrefs.SetInt(_playerProvince.recipeDoneKeyName[provinceCurrent - 1], currentRound);

        if (currentRound <= maximumRound)
        {
            // Go to the Next Round
            StartCoroutine(PlayAnimation(false));
        }
        else
        {
            // If Completed, Go to Main
            if (provinceCurrent == provinceUnlocked)
                PlayerPrefs.SetInt("ProvinceCompleted", provinceCompleted + 1);
            GoBackToMain();
        }

        successRound = false;
    }

    public void RestartRound()
    {
        // Restart the Round
        successRound = false;
        StartCoroutine(PlayAnimation(false));
    }

    public void GoBackToMain()
    {
        _audioManager.StopMusic();
        int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);

        if (successRound && currentRound <= maximumRound)
        {
            currentRound++;
            if (CheckCurrentRound()) 
                PlayerPrefs.SetInt(_playerProvince.recipeDoneKeyName[provinceCurrent - 1], currentRound);
        }

        successRound = false;
        kitchenUI.SetActive(false);
        roundFinishUI.SetActive(false);
        _levelLoad.levelId = provinceCurrent;
        _levelLoad.LoadBack(_levelLoad.mainScene);
    }

    private void OnApplicationQuit()
    {
        int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        if (successRound && currentRound < maximumRound)
        {
            currentRound++;
            if (CheckCurrentRound()) 
                PlayerPrefs.SetInt(_playerProvince.recipeDoneKeyName[provinceCurrent - 1], currentRound);
        }
    }
}