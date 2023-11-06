using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private List<string> objectsOnPlate = new List<string>();
    private List<DishInfo> dishes = new List<DishInfo>();
    private IngredientManager ingredientManager;
    private IngredientModule ingredientModule;
    private OrderManager orderManager;

    private void Start()
    {
        ingredientManager = GameObject.FindWithTag("ingredientManager").GetComponent<IngredientManager>();
        ingredientModule = GameObject.FindWithTag("ingredientModule").GetComponent<IngredientModule>();
        orderManager = GameObject.FindWithTag("orderManager").GetComponent<OrderManager>();
        dishes = Resources.LoadAll<DishInfo>("recipeInfo").ToList();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectsOnPlate.Add(collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsOnPlate.Remove(collision.gameObject.name);
    }

    private bool recipeMatch()
    {
        DishInfo dish = orderManager.currentOrderPrompt;

        if (dish == null || dish.recipe.Count != objectsOnPlate.Count)
        {
            return false;
        }

        objectsOnPlate.Sort();
        for (int i = 0; i < dish.recipe.Count; i++)
        {
            if (dish.recipe[i].name != objectsOnPlate[i])
            {
                return false;
            }
        }
        return true;
    }


    public void checkIngredients()
    {
        if (!orderManager.currentOrderPrompt)
        {
            return;
        }

        if (recipeMatch())
        {
            orderManager.currentOrderPrompt = null;
            Debug.Log("Correct Dish");
        }
        else
        {
            Debug.Log("Incorrect Dish");
            if ((PlayerPrefs.GetInt("GlobalLives", 3) > 0))
            {
                PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", 3) - 1);
                PlayerPrefs.Save();
            }
            else
            {
                // Automatically goes back to main menu if no more lives
                Debug.Log("No more lives.");
            }
        }

        foreach(GameObject ingredient in GameObject.FindGameObjectsWithTag("looseIngredient"))
        {
            Destroy(ingredient);
        }
    }
}
