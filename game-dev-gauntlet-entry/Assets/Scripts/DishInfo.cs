using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "recipe", menuName = "Recipes/New Recipe")]
public class DishInfo : ScriptableObject
{
    public List<IngredientInfo> recipe = new List<IngredientInfo>();
    public Sprite sprite;
    public Vector3 scale;
    public string description;
}
