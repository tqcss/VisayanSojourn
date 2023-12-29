using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    private List<DishInfo> dishes = new List<DishInfo>();
    private RecipeManager recipeManager;
    private SettleLevel settleLevel;
    public GameObject timerBar;
    public Text timerText;
    public Text orderText;
    public Image dishMonoImage;
    public Image dishColoredImage;

    private float timeDuration;
    private float timeLeft;
    public bool timerRunning = false;

    public DishInfo currentOrderPrompt;

    private void Start()
    {
        dishes = Resources.LoadAll<DishInfo>("recipeInfo").ToList();
        recipeManager = GameObject.FindGameObjectWithTag("recipeManager").GetComponent<RecipeManager>();
        settleLevel = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleLevel>();
    }

    public void ChangeOrderPrompt(DishInfo dish)
    {
        currentOrderPrompt = dish;
        orderText.text = currentOrderPrompt.name;
        dishMonoImage.sprite = currentOrderPrompt.sprite;
        dishColoredImage.sprite = currentOrderPrompt.sprite;

        timeDuration = 10 + (currentOrderPrompt.recipe.Count * 5);
    }

    public void StartTimer()
    {
        timeLeft = timeDuration;
        timerBar.transform.localScale = new Vector3(1, timerBar.transform.localScale.y, 0);
        timerRunning = true;
    }

    private void Update()
    {
        if (!timerRunning)
            return;
        
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.transform.localScale = new Vector3((timeLeft / timeDuration), timerBar.transform.localScale.y, 0);
            timerText.text = string.Format("{0:00}", timeLeft);
        }
        else
        {
            settleLevel.FinishRound(false);
            recipeManager.FailPlayer(); 
        }
    }
}
