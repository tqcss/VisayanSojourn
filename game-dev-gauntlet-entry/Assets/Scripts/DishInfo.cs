using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "recipe", menuName = "Recipes/New Recipe")]
public class DishInfo : ScriptableObject
{
    // All ingredients for a particular dish must be referenced in the inspector
    public List<IngredientInfo> recipe = new List<IngredientInfo>();
    public Sprite sprite;
    public Vector3 scale;
    public string description;
    public string shortDescription;
}
