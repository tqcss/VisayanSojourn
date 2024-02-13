using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    private List<DishInfo> dishes = new List<DishInfo>();
    public DishInfo currentOrderPrompt;
    public GameObject timerBar;
    public Text timerText;
    public Text orderText;
    public Image orderDisplay;
    public Image dishMonoImage;
    public Image dishColoredImage;
    public Text dishDescription;

    private float timeDuration;
    private float timeLeft;
    public bool timerRunning = false;

    private LevelLoad _levelLoad;
    private RecipeManager _recipeManager;
    private SettleKitchen _settleKitchen;
    private SettleRestaurant _settleRestaurant;

    private void Start()
    {
        // Reference the scripts from game objects
        dishes = Resources.LoadAll<DishInfo>("RecipeInfo").ToList();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _recipeManager = GameObject.FindGameObjectWithTag("recipeManager").GetComponent<RecipeManager>();
        _settleKitchen = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleKitchen>();
        _settleRestaurant = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleRestaurant>();
    }

    public void ChangeOrderPrompt(DishInfo dish)
    {
        // Change the order prompted from the DishList script
        currentOrderPrompt = dish;
        orderText.text = currentOrderPrompt.name;
        orderDisplay.sprite = currentOrderPrompt.sprite;
        dishColoredImage.sprite = currentOrderPrompt.sprite;

        if (_levelLoad.CheckModeId() == 1) 
        {
            dishMonoImage.sprite = currentOrderPrompt.sprite;
            dishDescription.text = currentOrderPrompt.shortDescription;
        }

        // Set the time duration based on the number of ingredients of a dish
        switch (_levelLoad.CheckModeId())
        {
            case 1:
                timeDuration = 8 + (currentOrderPrompt.recipe.Count * 4);
                break;
            case 2:
                timeDuration = 5 + (currentOrderPrompt.recipe.Count * 2.5f);
                break;
        }
    }

    public void StartTimer()
    {
        // Initiate the timer
        timeLeft = timeDuration;
        //timerBar.transform.localScale = new Vector3(1, timerBar.transform.localScale.y, 0);
        timerRunning = true;
    }

    private void Update()
    {
        if (!timerRunning)
            return;
        
        // Decrease the time left by deltaTime if it is more than 0
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            //timerBar.transform.localScale = new Vector3((timeLeft / timeDuration), timerBar.transform.localScale.y, 0);
            timerText.text = string.Format("{0:0.0}", timeLeft);
            
            if (_levelLoad.CheckModeId() == 2)
            {
                // The customer will be on sad emotion if the current time left is less than 1/4 of the time duration
                if (timeLeft < (timeDuration / 4.0f))
                {
                    switch (_settleRestaurant.genderId)
                    {
                        // FaceId: 1 - Sad
                        // Execute if the customer is a man
                        case 1:
                            _settleRestaurant.customerFace.sprite = _settleRestaurant.manFace[1];
                            break;
                        // Execute if the customer is a woman
                        case 2:
                            _settleRestaurant.customerFace.sprite = _settleRestaurant.womanFace[1];
                            break;
                    }
                }
            }
        }
        // Fail the player and end the round / order if the time reaches 0
        else
        {
            _recipeManager.FailPlayer();
            _recipeManager.SetEnd(false, true);
        }
    }

    public float SellCompute(float streakBonus, float provinceBonus)
    {
        float originalSellTotal = 0;
        float overallSellTotal = 0;

        // Accumulate all original sell points of each ingredient
        for (int i = 0; i < currentOrderPrompt.recipe.Count; i++)
            originalSellTotal += currentOrderPrompt.recipe[i].sellPoint;
        
        // Increase the sell point based on serve streak and current province bonuses
        overallSellTotal = (originalSellTotal * provinceBonus) * streakBonus;

        return Mathf.Round(overallSellTotal * 100.0f) * 0.01f;
    }
}
