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

    public int maximumRound;
    public int currentRound;



    private void Awake()
    {
        dishList = GameObject.FindGameObjectWithTag("dishList").GetComponent<DishList>();
        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        currentRound = 1;
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
        
        if (firstPlay) yield return new WaitForSeconds(scrollTimeSec);

        dishList.PromptOrder();
        startButton.SetActive(true);
        DisplayRecipe();
    }

    private void DisplayRecipe()
    {
        dishNameTextObj.SetActive(true);
        recipeTextObj.SetActive(true);
        dishMonoImage.SetActive(true);
        
        dishNameText.text = orderManager.currentOrderPrompt.name;
        string currentRecipeText = "Ingredients: \n";
        for (int i = 0; i < orderManager.currentOrderPrompt.recipe.Count; i++)
        {
            currentRecipeText = currentRecipeText + "  • " + orderManager.currentOrderPrompt.recipe[i].name.Replace("_", " ") + "\n";
            recipeText.text = currentRecipeText;
        }
    }

    public void StartRound()
    {
        startSfx.Play();
        recipeScroll.SetActive(false);
        kitchenUI.SetActive(true);
        orderManager.startTimer();
    }

    public void FinishRound()
    {
        roundFinishUI.SetActive(true);
        if (PlayerPrefs.GetInt("RoundSuccess", 0) == 1)
        {
            if ((PlayerPrefs.GetInt("GlobalLives", 3) > 0))
            {
                messageText.text = "ROUND SUCCESS";
                nextButton.SetActive(true);
                prevButton.SetActive(false);
                dishColoredImage.SetActive(true);
                if (currentRound < maximumRound) backButton.SetActive(true); else backButton.SetActive(false);
            }
            PlayerPrefs.SetInt("RoundSuccess", 0);
        }
        else if (PlayerPrefs.GetInt("RoundSuccess", 0) == 0)
        {
            if ((PlayerPrefs.GetInt("GlobalLives", 3) > 0))
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
            StartCoroutine(PlayAnimation(false));
        }
        else
        {
            Debug.Log("Go to Next Province");
            PlayerPrefs.SetInt("ProceedNext", 1);
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            PlayerPrefs.Save();
            kitchenUI.SetActive(false);
            roundFinishUI.SetActive(false);
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
        
        levelLoad.loadingProvinceText.text = levelLoad.loadingBgSprite[PlayerPrefs.GetInt("ProvinceCurrent", 1) - 1].name.Replace("image_", "").Replace("_", " ").ToUpper();
        levelLoad.loadingBg.sprite = levelLoad.loadingBgSprite[PlayerPrefs.GetInt("ProvinceCurrent", 1) - 1];
    }
}