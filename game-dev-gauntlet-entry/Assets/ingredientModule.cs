using UnityEngine;

public class ingredientModule : MonoBehaviour
{
    public ingredientInfo[] ingredients;

    public ingredientInfo getIngredient(int id)
    {
        return ingredients[id];
    }
}
