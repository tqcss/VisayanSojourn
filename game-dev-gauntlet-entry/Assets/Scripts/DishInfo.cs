using UnityEngine;

[CreateAssetMenu(fileName = "recipe", menuName = "recipes/newRecipe")]
public class DishInfo : ScriptableObject
{
    public int[] recipe;
    public Sprite sprite;
    public Vector3 scale;
}
