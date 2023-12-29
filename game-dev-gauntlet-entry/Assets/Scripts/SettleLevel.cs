using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettleLevel : MonoBehaviour
{
    public GameObject kitchenUI;
    public GameObject recipeScroll;
    public int scrollTimeSec;
    public GameObject startButton;
    public Text dishNameText;
    public GameObject dishNameTextObj;
    public Text recipeText;
    public GameObject recipeTextObj;
    public GameObject dishMonoImage;
    public GameObject roundFinishUI;
    public Text messageText;
    public GameObject backButton;
    public GameObject prevButton;
    public GameObject nextButton;
    public GameObject dishColoredImage;
    
    public AudioSource startSfx;
    private DishList dishList;
    private LevelLoad levelLoad;
    private OrderManager orderManager;
    private PlayerLives playerLives;
    private VideoRender videoRender;

    public int maximumRound;
    public int currentRound;

    private void Awake()
    {
        dishList = GameObject.FindGameObjectWithTag("dishList").GetComponent<DishList>();
        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
        
        currentRound = 1;
        PlayAnimation(true);
    }

    private void PlayAnimation(bool firstPlay)
    {
        recipeScroll.SetActive(true);
        dishNameTextObj.SetActive(false);
        recipeTextObj.SetActive(false);
        dishMonoImage.SetActive(false);
        startButton.SetActive(false);
        kitchenUI.SetActive(false);
        roundFinishUI.SetActive(false);
        levelLoad.loadingScreen.SetActive(false);
        
        if (firstPlay) videoRender.PlayScroll();
    }

    public void DisplayRecipe()
    {
        dishList.PromptOrder();
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
        orderManager.StartTimer();
    }

    public void FinishRound(bool success)
    {
        roundFinishUI.SetActive(true);
        if (success)
        {
            if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) > 0)
            {
                messageText.text = "ROUND SUCCESS";
                nextButton.SetActive(true);
                prevButton.SetActive(false);
                dishColoredImage.SetActive(true);
                if (currentRound < maximumRound) 
                    backButton.SetActive(true); 
                else 
                    backButton.SetActive(false);
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) > 0)
            {
                messageText.text = "ROUND FAIL";
                prevButton.SetActive(true);
            }
            else
            {
                messageText.text = "NO MORE LIVES";
                prevButton.SetActive(false);
            }
            nextButton.SetActive(false);
            backButton.SetActive(true);
            dishColoredImage.SetActive(false);
        }
    }

    public void OntoNextRound()
    {
        if (currentRound < maximumRound)
        {
            currentRound++;
            PlayAnimation(false);
        }
        else
        {
            if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == PlayerPrefs.GetInt("ProvinceCurrent", 1))
                PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            
            levelLoad.levelId = PlayerPrefs.GetInt("ProvinceCurrent", 1) - 1;
            levelLoad.LoadFinishBack(levelLoad.mainScene);
            kitchenUI.SetActive(false);
            roundFinishUI.SetActive(false);
        }
    }

    public void OntoPreviousRound()
    {
        PlayAnimation(false);
    }

    public void GoBackToMain()
    {
        kitchenUI.SetActive(false);
        roundFinishUI.SetActive(false);
        levelLoad.levelId = PlayerPrefs.GetInt("ProvinceCurrent", 1) - 1;
        levelLoad.LoadBack(levelLoad.mainScene);
    }
}