using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ingredientChecker : MonoBehaviour
{
    public Text textList;
    public ingredientManager ingredientManager;
    public ingredientModule ingredientModule;
    private List<int> list = new List<int>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!textList) return;
        list.Add(collision.gameObject.GetComponent<info>().id);
        textList.text = "";
        foreach (int i in list)
        {
            textList.text += ingredientModule.getIngredient(i).name + '\n';
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!textList) return;
        list.Remove(collision.gameObject.GetComponent<info>().id);
        textList.text = "";
        foreach (int i in list)
        {
            textList.text += ingredientModule.getIngredient(i).name + '\n';
        }
    }
}
