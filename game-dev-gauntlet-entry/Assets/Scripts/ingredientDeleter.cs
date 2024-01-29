using UnityEngine;

public class IngredientDeleter : MonoBehaviour
{
    private GameObject dishParticle;

    private AudioManager _audioManager;
    public IngredientModule ingredientModule;

    private void Start()
    {
        // Referencing the Scripts from GameObjects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        
        // Referencing the Prefab Particle
        dishParticle = Resources.Load("Prefabs/dishParticle", typeof(GameObject)) as GameObject;
    }

    private void OnCollisionEnter2D(Collision2D ingredient)
    {
        _audioManager.breakSfx.Play();

        // Instantiate dishParticle When an Ingredient Collides on Hitbox
        GameObject newParticle = Instantiate(dishParticle, ingredient.transform.position, Quaternion.identity);
        var maintemp = newParticle.GetComponent<ParticleSystem>().main;
        maintemp.startColor = new ParticleSystem.MinMaxGradient(ingredientModule.GetIngredient(ingredient.gameObject.name).particleColorA, ingredientModule.GetIngredient(ingredient.gameObject.name).particleColorB);
        
        Destroy(ingredient.gameObject);
        newParticle.GetComponent<ParticleSystem>().Play();
    }
}
