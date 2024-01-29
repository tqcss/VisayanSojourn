using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class IngredientModule : MonoBehaviour
{
    public List<IngredientInfo> ingredients = new List<IngredientInfo>();
    private List<IngredientInfo> allIngredients = new List<IngredientInfo>();

    private void Awake()
    {
        // Only Adds Ingredients Correspond to the Current Province
        allIngredients = Resources.LoadAll<IngredientInfo>("IngredientInfo").ToList();
        foreach (IngredientInfo ingredientInfo in allIngredients)
            if (ingredientInfo.isIncludedInProvince[PlayerPrefs.GetInt("ProvinceCurrent", 1) - 1])
                ingredients.Add(ingredientInfo);
    }

    public IngredientInfo GetIngredient(string name)
    {
        // Checking Ingredients
        foreach (IngredientInfo ingredient in ingredients)
            if (ingredient.name == name)
                return ingredient;    

        Debug.LogWarning($"No ingredient found with the name '{name}'");
        return null;
    }
}
