using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DishList : MonoBehaviour
{
    public int dishesLength;
    public DishInfo[] dishAntique;
    public DishInfo[] dishAklan;
    public DishInfo[] dishCapiz;
    public DishInfo[] dishNegrosOccidental;
    public DishInfo[] dishGuimaras;
    public DishInfo[] dishIloilo;

    private OrderManager _orderManager;
    private SettleKitchen _settleKitchen;

    private void Awake()
    {
        // Reference the scripts from game objects
        _orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        _settleKitchen = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleKitchen>();
    }

    public void PromptOrder() 
    {
        /*
            ProvinceCurrent
            1: Antique
            2: Aklan
            3: Capiz
            4: Negros Occidental
            5: Guimaras
            6: Iloilo
        */

        // Prompt a dish based on the current province and the current round,
        // then set the value of maximum round based on the number of dishes
        switch (PlayerPrefs.GetInt("ProvinceCurrent", 0))
        {
            case 1:
                _orderManager.ChangeOrderPrompt(dishAntique[_settleKitchen.currentRound - 1]);
                dishesLength = dishAntique.Length;
                break;
            case 2:
                _orderManager.ChangeOrderPrompt(dishAklan[_settleKitchen.currentRound - 1]);
                dishesLength = dishAklan.Length;
                break;
            case 3:
                _orderManager.ChangeOrderPrompt(dishCapiz[_settleKitchen.currentRound - 1]);
                dishesLength = dishCapiz.Length;
                break;
            case 4:
                _orderManager.ChangeOrderPrompt(dishNegrosOccidental[_settleKitchen.currentRound - 1]);
                dishesLength = dishNegrosOccidental.Length;
                break;
            case 5:
                _orderManager.ChangeOrderPrompt(dishGuimaras[_settleKitchen.currentRound - 1]);
                dishesLength = dishGuimaras.Length;
                break;
            case 6:
                _orderManager.ChangeOrderPrompt(dishIloilo[_settleKitchen.currentRound - 1]);
                dishesLength = dishIloilo.Length;
                break;
        }
        _settleKitchen.maximumRound = dishesLength;
    }

    public void RandomPromptOrder()
    {
        /*
            ProvinceCurrent
            1: Antique
            2: Aklan
            3: Capiz
            4: Negros Occidental
            5: Guimaras
            6: Iloilo
        */

        // Prompt a random dish from [0] to [dish[] length] based on the current province
        switch (PlayerPrefs.GetInt("ProvinceCurrent", 0))
        {
            case 1:
                _orderManager.ChangeOrderPrompt(dishAntique[Random.Range(0, dishAntique.Length)]);
                dishesLength = dishAntique.Length;
                break;
            case 2:
                _orderManager.ChangeOrderPrompt(dishAklan[Random.Range(0, dishAklan.Length)]);
                dishesLength = dishAklan.Length;
                break;
            case 3:
                _orderManager.ChangeOrderPrompt(dishCapiz[Random.Range(0, dishCapiz.Length)]);
                dishesLength = dishCapiz.Length;
                break;
            case 4:
                _orderManager.ChangeOrderPrompt(dishNegrosOccidental[Random.Range(0, dishNegrosOccidental.Length)]);
                dishesLength = dishNegrosOccidental.Length;
                break;
            case 5:
                _orderManager.ChangeOrderPrompt(dishGuimaras[Random.Range(0, dishGuimaras.Length)]);
                dishesLength = dishGuimaras.Length;
                break;
            case 6:
                _orderManager.ChangeOrderPrompt(dishIloilo[Random.Range(0, dishIloilo.Length)]);
                dishesLength = dishIloilo.Length;
                break;
        }
    }
}
