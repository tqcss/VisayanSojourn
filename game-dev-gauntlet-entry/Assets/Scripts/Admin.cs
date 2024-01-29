using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin : MonoBehaviour
{
    private LevelLoad _levelLoad;
    private PlayerCoins _playerCoins;
    private PlayerLives _playerLives;
    private PlayerProvince _playerProvince;
    
    private void Awake()
    {
        // Referencing the Scripts from GameObjects
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        _playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        _playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
    }

    public void DeleteSavedData()
    {
        // Delete the Data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void IncreaseLives()
    {
        // Increase life by one
        int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
        if (globalLives < _playerLives.livesMax)
            PlayerPrefs.SetInt("GlobalLives", globalLives + 1);
    }

    public void DecreaseLives()
    {
        // Decrease life by one
        int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
        if (globalLives > 0)
            PlayerPrefs.SetInt("GlobalLives", globalLives - 1);
    }

    public void IncreaseLevel()
    {
        // Increase level by one
        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        if (provinceCompleted != provinceUnlocked)
        {
            PlayerPrefs.SetInt("ProvinceCompleted", provinceCompleted + 1);
        }
        else if (provinceUnlocked < _playerProvince.provinceTotal)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", provinceUnlocked + 1);
            PlayerPrefs.SetInt(_levelLoad.firstTimeKeyName[provinceUnlocked - 1], 1);
        }
    }

    public void DecreaseLevel()
    {
        // Decrease level by one
        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        if (provinceCompleted != provinceUnlocked)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", provinceUnlocked - 1);
            PlayerPrefs.SetInt(_levelLoad.firstTimeKeyName[provinceUnlocked - 1], 1);
        }
        else if (provinceUnlocked > 0)
        {
            PlayerPrefs.SetInt("ProvinceCompleted", provinceCompleted - 1);
        }
    }

    public void IncreaseCoins()
    {
        // Increase coins by 10
        _playerCoins.IncreaseCoins(10.0f);
    }

    public void DecreaseCoins()
    {
        // Decrease coins by 10
        _playerCoins.DecreaseCoins(10.0f);
    }
}
