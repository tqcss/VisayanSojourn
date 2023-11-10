using UnityEngine;

[CreateAssetMenu(fileName = "ingredient", menuName = "ingredients/newIngredient")]
public class IngredientInfo : ScriptableObject
{
    public Sprite sprite;
    public static float scaleX = 2;
    public static float scaleY = 2;
    public float colliderSizeX;
    public float colliderSizeY;
    public bool randomRotation;
    public Color particleColorA;
    public Color particleColorB;

    private bool scaledDown = true;
    private float scaleDownX = scaleX * .8f;
    private float scaleDownY = scaleY * .8f;
}