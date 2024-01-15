using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin : MonoBehaviour
{
    private PlayerCoins playerCoins;
    private PlayerLives playerLives;
    private PlayerProvince playerProvince;
    
    private void Awake()
    {
        playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        playerProvince = GameObject.FindGameObjectWithTag("mainScript").GetComponent<PlayerProvince>();
    }

    public void DeleteSavedData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        playerCoins.UpdateDisplay();
        playerProvince.UpdateDisplay();
    }

    public void IncreaseLives()
    {
        if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) < playerLives.livesMax)
        {
            PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) + 1);
            PlayerPrefs.Save();
        }
    }

    public void DecreaseLives()
    {
        if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) > 0)
        {
            PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax) - 1);
            PlayerPrefs.Save();
        }
    }

    public void IncreaseLevel()
    {
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) < playerProvince.provinceTotal)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            PlayerPrefs.Save();
            playerProvince.UpdateDisplay();
        }
    }

    public void DecreaseLevel()
    {
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) > 1)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) - 1);
            PlayerPrefs.Save();
            playerProvince.UpdateDisplay();
        }
    }

    public void IncreaseCoins()
    {
        playerCoins.IncreaseCoins(10.0f);
        playerCoins.UpdateDisplay();
    }

    public void DecreaseCoins()
    {
        playerCoins.DecreaseCoins(10.0f);
        playerCoins.UpdateDisplay();
    }
}
