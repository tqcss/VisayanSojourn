using UnityEngine;

[CreateAssetMenu(fileName = "ingredient", menuName = "ingredients/newIngredient")]
public class IngredientInfo : ScriptableObject
{
    public Sprite sprite;
    public float scaleX;
    public float scaleY;
    public float colliderSizeX;
    public float colliderSizeY;
    public bool randomRotation;
    public Color particleColor;
}