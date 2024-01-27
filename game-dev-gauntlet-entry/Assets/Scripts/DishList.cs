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
    private OrderManager orderManager;
    private PlayerProvince playerProvince;
    private SettleKitchen settleKitchen;

    private void Awake()
    {
        orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
        settleKitchen = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleKitchen>();
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
        switch (PlayerPrefs.GetInt("ProvinceCurrent", 0))
        {
            case 1:
                orderManager.ChangeOrderPrompt(dishAntique[settleKitchen.currentRound - 1]);
                settleKitchen.maximumRound = dishAntique.Length;
                break;
            case 2:
                orderManager.ChangeOrderPrompt(dishAklan[settleKitchen.currentRound - 1]);
                settleKitchen.maximumRound = dishAklan.Length;
                break;
            case 3:
                orderManager.ChangeOrderPrompt(dishCapiz[settleKitchen.currentRound - 1]);
                settleKitchen.maximumRound = dishCapiz.Length;
                break;
            case 4:
                orderManager.ChangeOrderPrompt(dishNegrosOccidental[settleKitchen.currentRound - 1]);
                settleKitchen.maximumRound = dishNegrosOccidental.Length;
                break;
            case 5:
                orderManager.ChangeOrderPrompt(dishGuimaras[settleKitchen.currentRound - 1]);
                settleKitchen.maximumRound = dishGuimaras.Length;
                break;
            case 6:
                orderManager.ChangeOrderPrompt(dishIloilo[settleKitchen.currentRound - 1]);
                settleKitchen.maximumRound = dishIloilo.Length;
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
        switch (PlayerPrefs.GetInt("ProvinceCurrent", 0))
        {
            case 1:
                orderManager.ChangeOrderPrompt(dishAntique[Random.Range(0, dishAntique.Length)]);
                break;
            case 2:
                orderManager.ChangeOrderPrompt(dishAklan[Random.Range(0, dishAklan.Length)]);
                break;
            case 3:
                orderManager.ChangeOrderPrompt(dishCapiz[Random.Range(0, dishCapiz.Length)]);
                break;
            case 4:
                orderManager.ChangeOrderPrompt(dishNegrosOccidental[Random.Range(0, dishNegrosOccidental.Length)]);
                break;
            case 5:
                orderManager.ChangeOrderPrompt(dishGuimaras[Random.Range(0, dishGuimaras.Length)]);
                break;
            case 6:
                orderManager.ChangeOrderPrompt(dishIloilo[Random.Range(0, dishIloilo.Length)]);
                break;
        }
    }
}
