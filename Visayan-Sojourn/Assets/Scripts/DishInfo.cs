using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "recipe", menuName = "Recipes/New Recipe")]
public class DishInfo : ScriptableObject
{
    // All ingredients for a particular dish must be referenced in the inspector
    public List<IngredientInfo> recipe = new List<IngredientInfo>();
    public Sprite sprite;
    public Vector3 scale;
    public Sprite framedSprite;
    public string description;
    public string shortDescription;

    // Tick the box if the dish is included in a certain province
    /*
        ProvinceCurrent
        [0]: 1 - Antique
        [1]: 2 - Aklan
        [2]: 3 - Capiz
        [3]: 4 - Negros Occidental
        [4]: 5 - Guimaras
        [5]: 6 - Iloilo
    */
    public bool[] isDishAtProvince = new bool[6];
}
