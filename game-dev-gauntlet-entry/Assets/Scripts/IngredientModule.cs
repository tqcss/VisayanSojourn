using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class IngredientModule : MonoBehaviour
{
    public List<IngredientInfo> ingredients = new List<IngredientInfo>();

    private void Awake()
    {
        ingredients = Resources.LoadAll<IngredientInfo>("ingredientInfo").ToList();
    }

    public IngredientInfo getIngredient(string name)
    {
        foreach (IngredientInfo ingredient in ingredients)
        {
            if (ingredient.name == name)
            {
                return ingredient;
            }
        }
        Debug.LogWarning($"No ingredient found with the name '{name}'");
        return null;
    }
}
