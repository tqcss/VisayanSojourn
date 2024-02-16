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
    public GameObject heartEmpty;
    public int maximumRound;
    public int currentRound;
    private bool successRound = false;
    
    private AudioManager _audioManager;
    private CookBookScript _cookBookScript;
    private DishList _dishList;
    private IngredientManager _ingredientManager;
    private LevelLoad _levelLoad;
    private OrderManager _orderManager;
    private PlayerLives _playerLives;
    private PlayerProvince _playerProvince;
    private SettingsManager _settingsManager;
    private VideoRender _videoRender;

    private void Awake()
    {
        // Reference the scripts from game objects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _cookBookScript = GameObject.FindGameObjectWithTag("cookBookSystem").GetComponent<CookBookScript>();
        _dishList = GameObject.FindGameObjectWithTag("dishList").GetComponent<DishList>();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        _playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        _playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
        _settingsManager = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettingsManager>();
        _videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
        
        // Set initial values to the variables and set the game objects
        currentRound = (CheckCurrentRound()) ? PlayerPrefs.GetInt(_playerProvince.recipeDoneKeyName[PlayerPrefs.GetInt("ProvinceCurrent", 0) - 1], 1) : 1;
        skipButton.SetActive(false);
        StartCoroutine(PlayAnimation(true));
    }

    private bool CheckCurrentRound()
    {
        // Check if it is not on the previous round as the current unlocked province
        return PlayerPrefs.GetInt("ProvinceCurrent", 0) == PlayerPrefs.GetInt("ProvinceUnlocked", 1) &&
               PlayerPrefs.GetInt("ProvinceCurrent", 0) != PlayerPrefs.GetInt("ProvinceCompleted", 0);
    }

    private IEnumerator PlayAnimation(bool firstPlay)
    {
        // Set the game objects
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
            // Play the video of recipe scroll
            #if UNITY_ANDROID
                StartCoroutine(_videoRender.PlayScroll());
            #endif
            
            #if UNITY_STANDALONE_WIN
                StartCoroutine(_videoRender.PlayScroll());
                yield return new WaitForSeconds(2);
                skipButton.SetActive(true);
            #endif
        }
        else
        {
            DisplayRecipe();
        }
        
        yield return null;
    }

    public void DisplayRecipe()
    {
        // Set the game objects
        _dishList.PromptOrder();
        scroll.SetActive(true);
        skipButton.SetActive(false);
        dishNameTextObj.SetActive(true);
        recipeTextObj.SetActive(true);
        dishMonoImage.SetActive(true);
        backButton.SetActive(true);
        startButton.SetActive(true);
        
        // Display the dish and its recipe and sprite
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
        // Start if the start button is pressed
        _audioManager.PlayBackgroundMusic(_audioManager.kitchenMusic, true);
        _audioManager.startSfx.Play();

        recipeScroll.SetActive(false);
        kitchenUI.SetActive(true);
        livesText.text = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax).ToString();
        _orderManager.StartTimer();
    }

    public void EndRound(bool success)
    {
        // End if the check button is pressed
        _audioManager.StopMusic();
        _cookBookScript.CloseCookBook();
        _settingsManager.CloseSettings();
        
        roundFinishUI.SetActive(true);
        int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
        
        if (success)
        {
            successRound = true;
            if (globalLives > 0)
            {
                // Set the game objects
                messageText.text = endMessage[0];
                nextButton.SetActive(true);
                restartButton.SetActive(false);
                heartEmpty.SetActive(false);
                descriptionPanel.SetActive(true);
                homeButton.SetActive((currentRound < maximumRound) ? true : false);

                // Set the life cooldown reward depending on the no. of fails before success
                _playerLives.RewardLife(PlayerPrefs.GetInt("FailsBeforeSuccess", 0), CheckCurrentRound());
            }
        }
        else
        {
            successRound = false;
            // Set the game objects
            messageText.text = (globalLives > 0) ? endMessage[1] : endMessage[2];
            restartButton.SetActive((globalLives > 0) ? true : false);
            heartEmpty.SetActive((globalLives > 0) ? false : true);

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

        // Go to the next round if the player did not reach the maximum round
        if (currentRound <= maximumRound)
        {
            StartCoroutine(PlayAnimation(false));
        }
        // Go to the main menu if the player reached the maximum round
        else
        {
            if (provinceCurrent == provinceUnlocked)
                PlayerPrefs.SetInt("ProvinceCompleted", provinceCompleted + 1);
            GoBackToMain();
        }

        successRound = false;
    }

    public void RestartRound()
    {
        // Restart if the restart button is pressed
        successRound = false;
        StartCoroutine(PlayAnimation(false));
    }

    public void GoBackToMain()
    {
        _audioManager.StopMusic();
        int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);

        // Increase the no. of recipe done if the round is success and the player did not reach the maximum round
        if (successRound && currentRound <= maximumRound)
        {
            currentRound++;
            if (CheckCurrentRound()) 
                PlayerPrefs.SetInt(_playerProvince.recipeDoneKeyName[provinceCurrent - 1], currentRound);
        }

        // Set the game objects
        successRound = false;
        kitchenUI.SetActive(false);
        roundFinishUI.SetActive(false);
        _levelLoad.levelId = provinceCurrent;
        _levelLoad.LoadBack(_levelLoad.travelScene);
    }

    private void OnApplicationQuit()
    {
        int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        // Increase the no. of recipe done if the round is success and the player did not reach the maximum round
        if (successRound && currentRound < maximumRound)
        {
            currentRound++;
            if (CheckCurrentRound()) 
                PlayerPrefs.SetInt(_playerProvince.recipeDoneKeyName[provinceCurrent - 1], currentRound);
        }
    }
}