using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettleKitchen : MonoBehaviour
{
    public GameObject kitchenUI;
    public GameObject recipeScroll;
    public GameObject skipButton;
    public GameObject startButton;
    public Text dishNameText;
    public GameObject dishNameTextObj;
    public Text recipeText;
    public GameObject recipeTextObj;
    public GameObject dishMonoImage;
    public Text livesText;
    public GameObject roundFinishUI;
    public Text messageText;
    public GameObject homeButton;
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
        skipButton.SetActive(false);
        StartCoroutine(PlayAnimation(true));
    }

    private IEnumerator PlayAnimation(bool firstPlay)
    {
        recipeScroll.SetActive(true);
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
            yield return new WaitForSeconds(1);
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
        if (success)
        {
            if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) > 0)
            {
                messageText.text = "ROUND SUCCESS";
                nextButton.SetActive(true);
                prevButton.SetActive(false);
                dishColoredImage.SetActive(true);
                if (currentRound < maximumRound) 
                    homeButton.SetActive(true); 
                else 
                    homeButton.SetActive(false);
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) > 0)
            {
                messageText.text = "ROUND FAIL";
                prevButton.SetActive(true);
            }
            else
            {
                messageText.text = "NO MORE LIVES";
                prevButton.SetActive(false);
            }
            livesText.text = PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax).ToString();
            nextButton.SetActive(false);
            homeButton.SetActive(true);
            dishColoredImage.SetActive(false);
        }
    }

    public void OntoNextRound()
    {
        if (currentRound < maximumRound)
        {
            currentRound++;
            StartCoroutine(PlayAnimation(false));
        }
        else
        {
            if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) == PlayerPrefs.GetInt("ProvinceCurrent", 1))
            {
                PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
                PlayerPrefs.Save();
            }

            GoBackToMain();
        }
    }

    public void OntoPreviousRound()
    {
        StartCoroutine(PlayAnimation(false));
    }

    public void GoBackToMain()
    {
        kitchenUI.SetActive(false);
        roundFinishUI.SetActive(false);
        levelLoad.levelId = PlayerPrefs.GetInt("ProvinceCurrent", 1) - 1;
        levelLoad.LoadBack(levelLoad.mainScene);
    }
}