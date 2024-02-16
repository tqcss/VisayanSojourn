using UnityEngine;

[CreateAssetMenu(fileName = "ingredient", menuName = "Ingredients/New Ingredient")]
public class IngredientInfo : ScriptableObject
{
    public Sprite sprite;
    public float scaleX = 2;
    public float scaleY = 2;
    public float colliderSizeX;
    public float colliderSizeY;
    public bool randomRotation;
    public Color particleColorA;
    public Color particleColorB;
    
    // Tick the box if the ingredient is included in the dish of that province
    /*
        ProvinceCurrent
        [0]: 1 - Antique
        [1]: 2 - Aklan
        [2]: 3 - Capiz
        [3]: 4 - Negros Occidental
        [4]: 5 - Guimaras
        [5]: 6 - Iloilo
    */
    public bool[] isIncludedInProvince = new bool[6];
    
    // Each ingredient should have an original sell point
    public float sellPoint;
}