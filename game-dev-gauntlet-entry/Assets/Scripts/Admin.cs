using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin : MonoBehaviour
{
    private PlayerCoins playerCoins;
    private PlayerLives playerLives;
    private PlayerProvince playerProvince;
    private UpdateDisplayMain updateDisplayMain;
    
    private void Awake()
    {
        playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
        updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();
    }

    public void DeleteSavedData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void IncreaseLives()
    {
        int globalLives = PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax);
        if (globalLives < playerLives.livesMax)
            PlayerPrefs.SetInt("GlobalLives", globalLives + 1);
    }

    public void DecreaseLives()
    {
        int globalLives = PlayerPrefs.GetInt("GlobalLives", playerLives.livesMax);
        if (globalLives > 0)
            PlayerPrefs.SetInt("GlobalLives", globalLives - 1);
    }

    public void IncreaseLevel()
    {
        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        if (provinceCompleted != provinceUnlocked)
            PlayerPrefs.SetInt("ProvinceCompleted", provinceCompleted + 1);
        else if (provinceUnlocked < playerProvince.provinceTotal)
            PlayerPrefs.SetInt("ProvinceUnlocked", provinceUnlocked + 1);
    }

    public void DecreaseLevel()
    {
        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        if (provinceCompleted != provinceUnlocked)
            PlayerPrefs.SetInt("ProvinceUnlocked", provinceUnlocked - 1);
        else if (provinceUnlocked > 1)
            PlayerPrefs.SetInt("ProvinceCompleted", provinceCompleted - 1);
    }

    public void IncreaseCoins()
    {
        playerCoins.IncreaseCoins(10.0f);
    }

    public void DecreaseCoins()
    {
        playerCoins.DecreaseCoins(10.0f);
    }
}
