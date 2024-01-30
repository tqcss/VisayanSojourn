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
        // Reference the scripts from game objects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _ingredientModule = GameObject.FindGameObjectWithTag("ingredientModule").GetComponent<IngredientModule>();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        _playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        _settleKitchen = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleKitchen>();
        _settleRestaurant = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleRestaurant>();
        
        // Reference the prefab particle
        dishParticle = Resources.Load("Prefabs/dishParticle", typeof(GameObject)) as GameObject;
        // Load all dishes from the Resources/RecipeInfo folder
        dishes = Resources.LoadAll<DishInfo>("RecipeInfo").ToList();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add ingredients to the list of objects on plate that are on the plate
        objectsOnPlate.Add(collision.gameObject.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Remove ingredients to the list of objects on plate that are not on the plate
        objectsOnPlate.Remove(collision.gameObject.name);
    }

    private bool RecipeMatch()
    {
        DishInfo dish = _orderManager.currentOrderPrompt;

        // Checks if the no. of ingredients on the plate is matched to the no. of ingredients of the prompted dish
        if (dish.recipe.Count != objectsOnPlate.Count)
            return false;

        objectsOnPlate.Sort();
        // Checks if ingredients on the plate are matched to the recipe of the prompted dish
        for (int i = 0; i < dish.recipe.Count; i++)
            if (dish.recipe[i].name != objectsOnPlate[i])
                return false;
        
        return true;
    }

    public void CheckIngredients()
    {
        // Execute if the check button is pressed
        if (!_orderManager.currentOrderPrompt)
        {
            Debug.Log("No assigned dish");
            return;
        }

        if (RecipeMatch())
        {
            _audioManager.successSfx.Play();
            SetEnd(true);
        }
        else
        {
            FailPlayer();
            SetEnd(false);
        }
        
        _orderManager.timerRunning = false;
        _orderManager.currentOrderPrompt = null;
        DestroyAllLooseItems();
    }

    public void FailPlayer()
    {
        // Execute if the player failed
        _audioManager.failSfx.Play();
        _orderManager.timerRunning = false;
        DestroyAllLooseItems();
        
        if (_levelLoad.CheckModeId() == 1)
        {
            int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
            int failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);

            // Decrement the player global life by one and increment no. of fails before success by one
            if (globalLives > 0)
            {
                PlayerPrefs.SetInt("GlobalLives", globalLives - 1);
                PlayerPrefs.SetInt("FailsBeforeSuccess", failsBeforeSuccess + 1);
            }
        }
    }

    public void SetEnd(bool success)
    {
        // Check the mode id of the current active mode
        switch (_levelLoad.CheckModeId())
        {
            case 1:
                _settleKitchen.EndRound(success);
                break;
            case 2:
                _settleRestaurant.EndOrder(success);
                break;
        }
    }

    private void DestroyAllLooseItems()
    {
        _audioManager.breakSfx.Play();
        foreach (GameObject ingredient in GameObject.FindGameObjectsWithTag("looseIngredient"))
        {
            // Instantiate dish particle to all ingredients and destroy them when the round ends
            GameObject newParticle = Instantiate(dishParticle, ingredient.transform.position, Quaternion.identity);
            var maintemp = newParticle.GetComponent<ParticleSystem>().main;
            maintemp.startColor = new ParticleSystem.MinMaxGradient(_ingredientModule.GetIngredient(ingredient.name).particleColorA, _ingredientModule.GetIngredient(ingredient.name).particleColorB);
            
            Destroy(ingredient);
            newParticle.GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnApplicationQuit()
    {
        if (_levelLoad.CheckModeId() == 1)
        {
            if (_orderManager.timerRunning)
            {
                int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
                if (globalLives > 0)
                    // Decrease the player global life if the kitchen round is still active upon quitting
                    PlayerPrefs.SetInt("GlobalLives", globalLives - 1);
                
                PlayerPrefs.SetInt("FailsBeforeSuccess", 0);
                PlayerPrefs.Save();
            }
        }
    }
}
