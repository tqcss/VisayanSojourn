using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private List<int> objectsOnPlate = new List<int>();
    private IngredientManager ingredientManager;
    private IngredientModule ingredientModule;

    private void Start()
    {
        ingredientManager = GameObject.FindWithTag("ingredientManager").GetComponent<IngredientManager>();
        ingredientModule = GameObject.FindWithTag("ingredientModule").GetComponent<IngredientModule>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectsOnPlate.Add(collision.gameObject.GetComponent<ObjectInfo>().id);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsOnPlate.Remove(collision.gameObject.GetComponent<ObjectInfo>().id);
    }
}
