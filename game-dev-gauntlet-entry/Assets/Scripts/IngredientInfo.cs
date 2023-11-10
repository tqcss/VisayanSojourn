using UnityEngine;

[CreateAssetMenu(fileName = "ingredient", menuName = "ingredients/newIngredient")]
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
}