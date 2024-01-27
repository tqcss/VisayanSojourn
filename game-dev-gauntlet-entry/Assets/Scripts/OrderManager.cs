using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    private List<DishInfo> dishes = new List<DishInfo>();
    private LevelLoad levelLoad;
    private RecipeManager recipeManager;
    private SettleKitchen settleKitchen;
    private SettleRestaurant settleRestaurant;
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

    public DishInfo currentOrderPrompt;

    private void Start()
    {
        dishes = Resources.LoadAll<DishInfo>("RecipeInfo").ToList();
        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        recipeManager = GameObject.FindGameObjectWithTag("recipeManager").GetComponent<RecipeManager>();
        settleKitchen = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleKitchen>();
        settleRestaurant = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleRestaurant>();
    }

    public void ChangeOrderPrompt(DishInfo dish)
    {
        currentOrderPrompt = dish;
        orderText.text = currentOrderPrompt.name;
        orderDisplay.sprite = currentOrderPrompt.sprite;
        dishColoredImage.sprite = currentOrderPrompt.sprite;
        
        timeDuration = 8 + (currentOrderPrompt.recipe.Count * 4);

        if (SceneManager.GetActiveScene().name == levelLoad.kitchenScene) 
        {
            dishMonoImage.sprite = currentOrderPrompt.sprite;
            dishDescription.text = currentOrderPrompt.description;
        }
    }

    public float SellCompute()
    {
        float sellTotal = 0;
        for (int i = 0; i < currentOrderPrompt.recipe.Count; i++)
            sellTotal += currentOrderPrompt.recipe[i].sellPoint;
        return sellTotal;
    }

    public void StartTimer()
    {
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
            recipeManager.FailPlayer(); 
            
            if (SceneManager.GetActiveScene().name == levelLoad.kitchenScene) 
                settleKitchen.EndRound(false);
            else if (SceneManager.GetActiveScene().name == levelLoad.restaurantScene) 
                settleRestaurant.EndOrder(false);
        }
    }
}
