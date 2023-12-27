using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    private List<string> objectsOnPlate = new List<string>();
    private List<DishInfo> dishes = new List<DishInfo>();
    private IngredientManager ingredientManager;
    private IngredientModule ingredientModule;
    private OrderManager orderManager;
    private PlayerLives playerLives;
    private GameObject particles;

    public AudioSource successSfx;
    public AudioSource failSfx;
    public AudioSource destroySfx;
    public AudioSource bgMusic;

    public int particleColorOffset = 5;

    private void Start()
    {
        ingredientManager = GameObject.FindWithTag("ingredientManager").GetComponent<IngredientManager>();
        ingredientModule = GameObject.FindWithTag("ingredientModule").GetComponent<IngredientModule>();
        orderManager = GameObject.FindWithTag("orderManager").GetComponent<OrderManager>();
        playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        particles = Resources.Load("Prefabs/particleSystem", typeof(GameObject)) as GameObject;
        dishes = Resources.LoadAll<DishInfo>("recipeInfo").ToList();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectsOnPlate.Add(collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsOnPlate.Remove(collision.gameObject.name);
    }

    private bool recipeMatch()
    {
        DishInfo dish = orderManager.currentOrderPrompt;

        if (dish.recipe.Count != objectsOnPlate.Count)
            return false;

        objectsOnPlate.Sort();
        for (int i = 0; i < dish.recipe.Count; i++)
            if (dish.recipe[i].name != objectsOnPlate[i])
                return false;
        
        return true;
    }

    public void checkIngredients()
    {
        bgMusic.Stop();
        if (!orderManager.currentOrderPrompt)
        {
            Debug.Log("No assigned dish");
            return;
        }

        if (recipeMatch())
        {
            successSfx.Play();
            orderManager.currentOrderPrompt = null;
            PlayerPrefs.SetInt("RoundSuccess", 1);
        }
        else
        {
            failPlayer();
            PlayerPrefs.SetInt("RoundSuccess", 0);
        }
        orderManager.timerRunning = false;
        orderManager.currentOrderPrompt = null;
        destroyAllLooseItems();
    }

    public void failPlayer()
    {
        bgMusic.Stop();
        failSfx.Play();
        orderManager.timerRunning = false;
        destroyAllLooseItems();
        if ((PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) > 0))
        {
            PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) - 1);
            PlayerPrefs.SetInt("FailsBeforeWin", PlayerPrefs.GetInt("FailsBeforeWin", 0) + 1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("No more lives.");
        }
    }

    private void destroyAllLooseItems()
    {
        destroySfx.Play();
        foreach (GameObject ingredient in GameObject.FindGameObjectsWithTag("looseIngredient"))
        {
            GameObject newParticle = Instantiate(particles, ingredient.transform.position, Quaternion.identity);
            var maintemp = newParticle.GetComponent<ParticleSystem>().main;
            maintemp.startColor = new ParticleSystem.MinMaxGradient(ingredientModule.getIngredient(ingredient.name).particleColorA, ingredientModule.getIngredient(ingredient.name).particleColorB);
            Destroy(ingredient);
            newParticle.GetComponent<ParticleSystem>().Play();
        }
    }
}
