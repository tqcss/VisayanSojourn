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
        // Referencing the Scripts from GameObjects
        dishes = Resources.LoadAll<DishInfo>("RecipeInfo").ToList();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _recipeManager = GameObject.FindGameObjectWithTag("recipeManager").GetComponent<RecipeManager>();
        _settleKitchen = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleKitchen>();
        _settleRestaurant = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleRestaurant>();
    }

    public void ChangeOrderPrompt(DishInfo dish)
    {
        // Change Order Prompt
        currentOrderPrompt = dish;
        orderText.text = currentOrderPrompt.name;
        orderDisplay.sprite = currentOrderPrompt.sprite;
        dishColoredImage.sprite = currentOrderPrompt.sprite;
        
        timeDuration = 8 + (currentOrderPrompt.recipe.Count * 4);

        if (SceneManager.GetActiveScene().name == _levelLoad.kitchenScene) 
        {
            dishMonoImage.sprite = currentOrderPrompt.sprite;
            dishDescription.text = currentOrderPrompt.description;
        }
    }

    public void StartTimer()
    {
        // Initiate Timer
        timeLeft = timeDuration;
        //timerBar.transform.localScale = new Vector3(1, timerBar.transform.localScale.y, 0);
        timerRunning = true;
    }

    private void Update()
    {
        if (!timerRunning)
            return;
        
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            //timerBar.transform.localScale = new Vector3((timeLeft / timeDuration), timerBar.transform.localScale.y, 0);
            timerText.text = string.Format("{0:0.0}", timeLeft);
        }
        else
        {
            _recipeManager.FailPlayer(); 
            
            if (SceneManager.GetActiveScene().name == _levelLoad.kitchenScene) 
                _settleKitchen.EndRound(false);
            else if (SceneManager.GetActiveScene().name == _levelLoad.restaurantScene) 
                _settleRestaurant.EndOrder(false);
        }
    }

    public float SellCompute()
    {
        // (Restaurant Mode)
        // Accumulate all sellPoints of Each Ingredients
        float sellTotal = 0;
        for (int i = 0; i < currentOrderPrompt.recipe.Count; i++)
            sellTotal += currentOrderPrompt.recipe[i].sellPoint;
        return sellTotal;
    }
}
