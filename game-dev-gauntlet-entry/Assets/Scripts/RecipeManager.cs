using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecipeManager : MonoBehaviour
{
    private List<string> objectsOnPlate = new List<string>();
    private List<DishInfo> dishes = new List<DishInfo>();
    private GameObject dishParticle;
    public int particleColorOffset = 5;
    
    private AudioManager _audioManager;
    private IngredientModule _ingredientModule;
    private LevelLoad _levelLoad;
    private OrderManager _orderManager;
    private PlayerLives _playerLives;
    private SettleKitchen _settleKitchen;
    private SettleRestaurant _settleRestaurant;

    private void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _ingredientModule = GameObject.FindGameObjectWithTag("ingredientModule").GetComponent<IngredientModule>();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        _playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        _settleKitchen = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleKitchen>();
        _settleRestaurant = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleRestaurant>();
        
        dishParticle = Resources.Load("Prefabs/dishParticle", typeof(GameObject)) as GameObject;
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
        // Checks if Ingredients on the Plate are Matched to the Prompted Dish's Recipe
        DishInfo dish = _orderManager.currentOrderPrompt;

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
        // Execute if checkButton was Pressed
        if (!_orderManager.currentOrderPrompt)
        {
            Debug.Log("No assigned dish");
            return;
        }

        if (RecipeMatch())
        {
            _audioManager.successSfx.Play();
            
            if (SceneManager.GetActiveScene().name == _levelLoad.kitchenScene) 
                _settleKitchen.EndRound(true);
            else if (SceneManager.GetActiveScene().name == _levelLoad.restaurantScene) 
                _settleRestaurant.EndOrder(true);
        }
        else
        {
            FailPlayer();
            
            if (SceneManager.GetActiveScene().name == _levelLoad.kitchenScene) 
                _settleKitchen.EndRound(false);
            else if (SceneManager.GetActiveScene().name == _levelLoad.restaurantScene) 
                _settleRestaurant.EndOrder(false);
        }
        _orderManager.timerRunning = false;
        _orderManager.currentOrderPrompt = null;
        DestroyAllLooseItems();
    }

    public void FailPlayer()
    {
        // Execute if Player Failed
        _audioManager.failSfx.Play();
        _orderManager.timerRunning = false;
        DestroyAllLooseItems();
        
        if (SceneManager.GetActiveScene().name == _levelLoad.kitchenScene)
        {
            if (PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax) > 0)
            {
                PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax) - 1);
                PlayerPrefs.SetInt("FailsBeforeSuccess", PlayerPrefs.GetInt("FailsBeforeSuccess", 0) + 1);
            }
        }
    }

    private void DestroyAllLooseItems()
    {
        _audioManager.breakSfx.Play();
        foreach (GameObject ingredient in GameObject.FindGameObjectsWithTag("looseIngredient"))
        {
            // Instantiate dishParticle When an Ingredient Collides on Hitbox
            GameObject newParticle = Instantiate(dishParticle, ingredient.transform.position, Quaternion.identity);
            var maintemp = newParticle.GetComponent<ParticleSystem>().main;
            maintemp.startColor = new ParticleSystem.MinMaxGradient(_ingredientModule.GetIngredient(ingredient.name).particleColorA, _ingredientModule.GetIngredient(ingredient.name).particleColorB);
            
            Destroy(ingredient);
            newParticle.GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name == _levelLoad.kitchenScene)
        {
            if (_orderManager.timerRunning)
            {
                if (PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax) > 0)
                    PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax) - 1);
                
                PlayerPrefs.SetInt("FailsBeforeSuccess", 0);
                PlayerPrefs.Save();
            }
        }
    }
}
