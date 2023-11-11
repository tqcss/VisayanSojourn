using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    private List<DishInfo> dishes = new List<DishInfo>();
    public RecipeManager recipeManager;
    public SettleLevel settleLevel;
    public GameObject timerBar;
    public Text orderText;

    public float timeDuration;

    public DishInfo currentOrderPrompt;

    public bool timerRunning = false;
    private float timeLeft;



    void Start()
    {
        dishes = Resources.LoadAll<DishInfo>("recipeInfo").ToList();
        recipeManager = GameObject.FindGameObjectWithTag("recipeManager").GetComponent<RecipeManager>();
    }

    public void changeOrderPrompt(DishInfo dish)
    {
        currentOrderPrompt = dish;
        orderText.text = currentOrderPrompt.name;
        startTimer();
    }

    public void startTimer()
    {
        Debug.Log("Timer Started");
        timeLeft = timeDuration;
        timerBar.transform.localScale = new Vector3(1, timerBar.transform.localScale.y, 0);
        timerRunning = true;
    }

    void Update()
    {
        if (!timerRunning)
        {
            return;
        }
        
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.transform.localScale = new Vector3((timeLeft / timeDuration), timerBar.transform.localScale.y, 0);
        } else
        {
            settleLevel.FinishRound();
            recipeManager.failPlayer();
            
        }
    }
}
