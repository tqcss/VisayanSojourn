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
    private SettleLevel settleLevel;

    private void Awake()
    {
        orderManager = GameObject.FindGameObjectWithTag("orderManager").GetComponent<OrderManager>();
        settleLevel = GameObject.FindGameObjectWithTag("mainScript").GetComponent<SettleLevel>();
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
        if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 1)
        {
            orderManager.changeOrderPrompt(dishAntique[settleLevel.currentRound - 1]);
            settleLevel.maximumRound = dishAntique.Length;
        }
        else if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 2)
        {
            orderManager.changeOrderPrompt(dishAklan[settleLevel.currentRound - 1]);
            settleLevel.maximumRound = dishAklan.Length;
        }
        else if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 3)
        {
            orderManager.changeOrderPrompt(dishCapiz[settleLevel.currentRound - 1]);
            settleLevel.maximumRound = dishCapiz.Length;
        }
        else if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 4)
        {
            orderManager.changeOrderPrompt(dishNegrosOccidental[settleLevel.currentRound - 1]);
            settleLevel.maximumRound = dishNegrosOccidental.Length;
        }
        else if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 5)
        {
            orderManager.changeOrderPrompt(dishGuimaras[settleLevel.currentRound - 1]);
            settleLevel.maximumRound = dishGuimaras.Length;
        }
        else if (PlayerPrefs.GetInt("ProvinceCurrent", 1) == 6)
        {
            orderManager.changeOrderPrompt(dishIloilo[settleLevel.currentRound - 1]);
            settleLevel.maximumRound = dishIloilo.Length;
        }
    }
}
