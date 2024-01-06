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
    private SettleLevel settleLevel;
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
        settleLevel = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleLevel>();
        particles = Resources.Load("Prefabs/particleSystem", typeof(GameObject)) as GameObject;
        dishes = Resources.LoadAll<DishInfo>("RecipeInfo").ToList();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectsOnPlate.Add(collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsOnPlate.Remove(collision.gameObject.name);
    }

    private bool RecipeMatch()
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

    public void CheckIngredients()
    {
        bgMusic.Stop();
        if (!orderManager.currentOrderPrompt)
        {
            Debug.Log("No assigned dish");
            return;
        }

        if (RecipeMatch())
        {
            successSfx.Play();
            orderManager.currentOrderPrompt = null;
            settleLevel.FinishRound(true);
        }
        else
        {
            FailPlayer();
            settleLevel.FinishRound(false);
        }
        orderManager.timerRunning = false;
        orderManager.currentOrderPrompt = null;
        DestroyAllLooseItems();
    }

    public void FailPlayer()
    {
        bgMusic.Stop();
        failSfx.Play();
        orderManager.timerRunning = false;
        DestroyAllLooseItems();
        
        if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) > 0)
        {
            PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) - 1);
            PlayerPrefs.SetInt("FailsBeforeWin", PlayerPrefs.GetInt("FailsBeforeWin", 0) + 1);
            PlayerPrefs.Save();
        }
    }

    private void DestroyAllLooseItems()
    {
        destroySfx.Play();
        foreach (GameObject ingredient in GameObject.FindGameObjectsWithTag("looseIngredient"))
        {
            GameObject newParticle = Instantiate(particles, ingredient.transform.position, Quaternion.identity);
            var maintemp = newParticle.GetComponent<ParticleSystem>().main;
            maintemp.startColor = new ParticleSystem.MinMaxGradient(ingredientModule.GetIngredient(ingredient.name).particleColorA, ingredientModule.GetIngredient(ingredient.name).particleColorB);
            Destroy(ingredient);
            newParticle.GetComponent<ParticleSystem>().Play();
        }
    }
}
