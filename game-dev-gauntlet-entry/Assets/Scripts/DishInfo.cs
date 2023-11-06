using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "recipe", menuName = "recipes/newRecipe")]
public class DishInfo : ScriptableObject
{
    public List<IngredientInfo> recipe = new List<IngredientInfo>();
    public Sprite sprite;
    public Vector3 scale;
}
