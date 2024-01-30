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
        // Load all ingredients from the Resources/IngredientInfo folder
        allIngredients = Resources.LoadAll<IngredientInfo>("IngredientInfo").ToList();

        // Only adds ingredients to the list that correspond to the current province
        foreach (IngredientInfo ingredientInfo in allIngredients)
            if (ingredientInfo.isIncludedInProvince[PlayerPrefs.GetInt("ProvinceCurrent", 1) - 1])
                ingredients.Add(ingredientInfo);
    }

    public IngredientInfo GetIngredient(string name)
    {
        // Check all loaded ingredients
        foreach (IngredientInfo ingredient in ingredients)
            if (ingredient.name == name)
                return ingredient;    

        // Console warning if an ingredient is not found
        Debug.LogWarning($"No ingredient found with the name '{name}'");
        return null;
    }
}
