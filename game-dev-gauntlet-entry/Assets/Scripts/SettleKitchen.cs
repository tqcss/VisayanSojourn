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
    private bool successRound = false;
    
    public AudioSource startSfx;
    private DishList dishList;
    private LevelLoad levelLoad;
    private OrderManager orderManager;
    private PlayerLives playerLives;
    private PlayerProvince playerProvince;
    private VideoRender videoRender;

    public int maximumRound;
    public int currentRound;

    private void Awake()
    {
        dishList = GameObject.FindGameObjectWithTag("dishList").GetComponent<DishList>();
        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
        videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
        
        currentRound = (CheckCurrentRound()) ? PlayerPrefs.GetInt(playerProvince.recipeDoneKeyName[PlayerPrefs.GetInt("ProvinceCurrent", 0) - 1], 1) : 1;
        skipButton.SetActive(false);
        StartCoroutine(PlayAnimation(true));
    }

    private bool CheckCurrentRound()
    {
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
        startButton.SetActive(false);
        kitchenUI.SetActive(false);
        roundFinishUI.SetActive(false);
        levelLoad.loadingScreen.SetActive(false);
        
        if (firstPlay) 
        {
            videoRender.PlayScroll();
            yield return new WaitForSeconds(2);
            skipButton.SetActive(true);
        }
        else 
            DisplayRecipe();
        
        yield return null;
    }

    public void DisplayRecipe()
    {
        dishList.PromptOrder();
        skipButton.SetActive(false);
        dishNameTextObj.SetActive(true);
        recipeTextObj.SetActive(true);
        dishMonoImage.SetActive(true);
        startButton.SetActive(true);
        
        dishNameText.text = orderManager.currentOrderPrompt.name;
        string currentRecipeText = "Ingredients: \n";
        for (int i = 0; i < orderManager.currentOrderPrompt.recipe.Count; i++)
        {
            currentRecipeText = currentRecipeText + "  • " + 
                                orderManager.currentOrderPrompt.recipe[i].name.Replace("_", " ") + "\n";
            recipeText.text = currentRecipeText;
        }
    }

    public void StartRound()
    {
        startSfx.Play();
        recipeScroll.SetActive(false);
        kitchenUI.SetActive(true);
        livesText.text = PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax).ToString();
        orderManager.StartTimer();
    }

    public void EndRound(bool success)
    {
        roundFinishUI.SetActive(true);
        int globalLives = PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax);
        
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

                playerLives.RewardLife(PlayerPrefs.GetInt("FailsBeforeSuccess", 0));
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
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        currentRound++;
        if (CheckCurrentRound()) 
            PlayerPrefs.SetInt(playerProvince.recipeDoneKeyName[provinceCurrent - 1], currentRound);

        if (currentRound <= maximumRound)
            StartCoroutine(PlayAnimation(false));
        else
        {
            PlayerPrefs.SetInt("ProvinceCompleted", PlayerPrefs.GetInt("ProvinceCompleted", 0) + 1);
            GoBackToMain();
        }

        successRound = false;
    }

    public void RestartRound()
    {
        successRound = false;
        StartCoroutine(PlayAnimation(false));
    }

    public void GoBackToMain()
    {
        int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        if (successRound = true && currentRound <= maximumRound)
        {
            currentRound++;
            if (CheckCurrentRound()) 
                PlayerPrefs.SetInt(playerProvince.recipeDoneKeyName[provinceCurrent - 1], currentRound);
        }

        successRound = false;
        kitchenUI.SetActive(false);
        roundFinishUI.SetActive(false);
        levelLoad.levelId = provinceCurrent;
        levelLoad.LoadBack(levelLoad.mainScene);
    }

    private void OnApplicationQuit()
    {
        int provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        if (successRound = true && currentRound < maximumRound)
        {
            currentRound++;
            if (CheckCurrentRound()) 
                PlayerPrefs.SetInt(playerProvince.recipeDoneKeyName[provinceCurrent - 1], currentRound);
        }
    }
}