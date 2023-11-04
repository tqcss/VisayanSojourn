using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class IngredientModule : MonoBehaviour
{
    public List<IngredientInfo> ingredients = new List<IngredientInfo>();

    private void Start()
    {
        ingredients = Resources.LoadAll<IngredientInfo>("ingredientInfo").ToList();
    }

    public IngredientInfo getIngredient(int id)
    {
        return ingredients[id];
    }
}
