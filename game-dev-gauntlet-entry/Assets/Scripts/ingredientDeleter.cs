using UnityEngine;

public class ingredientDeleter : MonoBehaviour
{
    public GameObject particles;
    public IngredientModule ingredientModule;
    public AudioSource destroySfx;

    private void OnCollisionEnter2D(Collision2D ingredient)
    {
        destroySfx.Play();
        GameObject newParticle = Instantiate(particles, ingredient.transform.position, Quaternion.identity);
        var maintemp = newParticle.GetComponent<ParticleSystem>().main;
        maintemp.startColor = new ParticleSystem.MinMaxGradient(ingredientModule.getIngredient(ingredient.gameObject.name).particleColorA, ingredientModule.getIngredient(ingredient.gameObject.name).particleColorB);
        Destroy(ingredient.gameObject);
        newParticle.GetComponent<ParticleSystem>().Play();
    }
}
