using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecipeManager : MonoBehaviour
{
    private List<string> objectsOnPlate = new List<string>();
    private List<DishInfo> dishes = new List<DishInfo>();
    private IngredientManager ingredientManager;
    private IngredientModule ingredientModule;
    private LevelLoad levelLoad;
    private OrderManager orderManager;
    private PlayerLives playerLives;
    private SettleKitchen settleKitchen;
    private SettleRestaurant settleRestaurant;
    private GameObject particles;

    public AudioSource successSfx;
    public AudioSource failSfx;
    public AudioSource destroySfx;
    public AudioSource bgMusic;

    public int particleColorOffset = 5;

    private void Start()
    {
        ingredientManager = GameObject.FindGameObjectWithTag("ingredientManager").GetComponent<IngredientManager>();
        ingredientModule = GameObject.FindGameObjectWithTag("ingredientModule").GetComponent<IngredientModule>();
        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        settleKitchen = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleKitchen>();
        settleRestaurant = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleRestaurant>();
        
        particles = Resources.Load("Prefabs/dishParticle", typeof(GameObject)) as GameObject;
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
            
            if (SceneManager.GetActiveScene().name == levelLoad.kitchenScene) 
                settleKitchen.EndRound(true);
            else if (SceneManager.GetActiveScene().name == levelLoad.restaurantScene) 
                settleRestaurant.EndOrder(true);
        }
        else
        {
            FailPlayer();
            
            if (SceneManager.GetActiveScene().name == levelLoad.kitchenScene) 
                settleKitchen.EndRound(false);
            else if (SceneManager.GetActiveScene().name == levelLoad.restaurantScene) 
                settleRestaurant.EndOrder(false);
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
        
        if (SceneManager.GetActiveScene().name == levelLoad.kitchenScene)
        {
            if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) > 0)
            {
                PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) - 1);
                PlayerPrefs.SetInt("FailsBeforeSuccess", PlayerPrefs.GetInt("FailsBeforeSuccess", 0) + 1);
            }
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

    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name == levelLoad.kitchenScene)
        {
            if (orderManager.timerRunning)
            {
                if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) > 0)
                    PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) - 1);
                
                PlayerPrefs.SetInt("FailsBeforeSuccess", 0);
                PlayerPrefs.Save();
            }
        }
    }
}
