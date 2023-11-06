using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    private List<DishInfo> dishes = new List<DishInfo>();
    private RecipeManager recipeManager;
    public Text orderText;

    public DishInfo currentOrderPrompt;

    void Start()
    {
        dishes = Resources.LoadAll<DishInfo>("recipeInfo").ToList();
        orderText = GameObject.FindGameObjectWithTag("orderText").GetComponent<Text>();
        recipeManager = GameObject.FindGameObjectWithTag("recipeManager").GetComponent<RecipeManager>();
    }

    public void changeOrderPrompt(DishInfo dish)
    {
        currentOrderPrompt = dish;
        orderText.text = currentOrderPrompt.name;
    }
}
