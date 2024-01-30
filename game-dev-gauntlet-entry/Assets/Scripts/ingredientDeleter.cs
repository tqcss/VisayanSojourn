using UnityEngine;

public class IngredientDeleter : MonoBehaviour
{
    private GameObject dishParticle;

    private AudioManager _audioManager;
    public IngredientModule ingredientModule;

    private void Start()
    {
        // Reference the scripts from game objects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        
        // Reference the prefab particle
        dishParticle = Resources.Load("Prefabs/dishParticle", typeof(GameObject)) as GameObject;
    }

    private void OnCollisionEnter2D(Collision2D ingredient)
    {
        _audioManager.breakSfx.Play();

        // Instantiate dish particle when an ingredient collides on the hitbox (box collider),
        // and destroy the ingredient game object
        GameObject newParticle = Instantiate(dishParticle, ingredient.transform.position, Quaternion.identity);
        var maintemp = newParticle.GetComponent<ParticleSystem>().main;
        maintemp.startColor = new ParticleSystem.MinMaxGradient(ingredientModule.GetIngredient(ingredient.gameObject.name).particleColorA, ingredientModule.GetIngredient(ingredient.gameObject.name).particleColorB);
        
        Destroy(ingredient.gameObject);
        newParticle.GetComponent<ParticleSystem>().Play();
    }
}
