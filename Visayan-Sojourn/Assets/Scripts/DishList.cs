using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DishList : MonoBehaviour
{
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
                _settleKitchen.maximumRound = dishAntique.Length;
                break;
            case 2:
                _orderManager.ChangeOrderPrompt(dishAklan[_settleKitchen.currentRound - 1]);
                _settleKitchen.maximumRound = dishAklan.Length;
                break;
            case 3:
                _orderManager.ChangeOrderPrompt(dishCapiz[_settleKitchen.currentRound - 1]);
                _settleKitchen.maximumRound = dishCapiz.Length;
                break;
            case 4:
                _orderManager.ChangeOrderPrompt(dishNegrosOccidental[_settleKitchen.currentRound - 1]);
                _settleKitchen.maximumRound = dishNegrosOccidental.Length;
                break;
            case 5:
                _orderManager.ChangeOrderPrompt(dishGuimaras[_settleKitchen.currentRound - 1]);
                _settleKitchen.maximumRound = dishGuimaras.Length;
                break;
            case 6:
                _orderManager.ChangeOrderPrompt(dishIloilo[_settleKitchen.currentRound - 1]);
                _settleKitchen.maximumRound = dishIloilo.Length;
                break;
        }
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
                break;
            case 2:
                _orderManager.ChangeOrderPrompt(dishAklan[Random.Range(0, dishAklan.Length)]);
                break;
            case 3:
                _orderManager.ChangeOrderPrompt(dishCapiz[Random.Range(0, dishCapiz.Length)]);
                break;
            case 4:
                _orderManager.ChangeOrderPrompt(dishNegrosOccidental[Random.Range(0, dishNegrosOccidental.Length)]);
                break;
            case 5:
                _orderManager.ChangeOrderPrompt(dishGuimaras[Random.Range(0, dishGuimaras.Length)]);
                break;
            case 6:
                _orderManager.ChangeOrderPrompt(dishIloilo[Random.Range(0, dishIloilo.Length)]);
                break;
        }
    }
}
