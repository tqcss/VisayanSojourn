using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    private List<DishInfo> dishes = new List<DishInfo>();
    private RecipeManager recipeManager;
    private Text orderText;

    private DishInfo currentOrderPrompt;

    void Start()
    {
        dishes = Resources.LoadAll<DishInfo>("recipeInfo").ToList();
        orderText = GameObject.FindGameObjectWithTag("orderText").GetComponent<Text>();
        recipeManager = GameObject.FindGameObjectWithTag("recipeManager").GetComponent<RecipeManager>();
    }

    public DishInfo getRandomDish()
    {
        currentOrderPrompt = dishes[Random.Range(0, dishes.Count)];
        orderText.text = currentOrderPrompt.name;
        return currentOrderPrompt;
    }


}
