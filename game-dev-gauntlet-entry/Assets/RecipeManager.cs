using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private List<int> objectsOnPlate = new List<int>();
    private List<DishInfo> dishes = new List<DishInfo>();
    private IngredientManager ingredientManager;
    private IngredientModule ingredientModule;

    private void Start()
    {
        ingredientManager = GameObject.FindWithTag("ingredientManager").GetComponent<IngredientManager>();
        ingredientModule = GameObject.FindWithTag("ingredientModule").GetComponent<IngredientModule>();
        dishes = Resources.LoadAll<DishInfo>("recipeInfo").ToList();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Add: " + collision.gameObject.GetComponent<ObjectInfo>().id);
        objectsOnPlate.Add(collision.gameObject.GetComponent<ObjectInfo>().id);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Remove: " + collision.gameObject.GetComponent<ObjectInfo>().id);
        objectsOnPlate.Remove(collision.gameObject.GetComponent<ObjectInfo>().id);
    }

    private bool recipeMatch(DishInfo dish)
    {
        if (dish.recipe.Length != objectsOnPlate.Count)
        {
            return false;
        }

        objectsOnPlate.Sort();
        for (int i = 0; i < dish.recipe.Length; i++)
        {
            if (dish.recipe[i] != objectsOnPlate[i])
            {
                return false;
            }
        }
        return true;
    }

    private DishInfo findDish()
    {
        // List<DishInfo> matchingDishes = dishes.Where(dish => objectsOnPlate.All(ingredient => dish.recipe.Contains(ingredient))).ToList();
        foreach (DishInfo dish in dishes)
        {
            if (recipeMatch(dish))
            {
                return dish;
            }
        }
        Debug.Log($"No dish found with recipe [{string.Join(", ", objectsOnPlate)}]");
        return null;
    }

    public void checkIngredients()
    {
        DishInfo dish = findDish();
        if (dish) Debug.Log("Dish found: " + dish.name);

        foreach(GameObject ingredient in GameObject.FindGameObjectsWithTag("looseIngredient"))
        {
            Destroy(ingredient);
        }
    }
}
