using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin : MonoBehaviour
{
    private LevelLoad _levelLoad;
    private PlayerCoins _playerCoins;
    private PlayerLives _playerLives;
    private PlayerProvince _playerProvince;
    private UpdateDisplayMain _updateDisplayMain;
    
    private void Awake()
    {
        // Reference the scripts from game objects
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        _playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        _playerProvince = GameObject.FindGameObjectWithTag("playerProvince").GetComponent<PlayerProvince>();
        _updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();
    }

    public void DeleteSavedData()
    {
        // Delete the saved data from player prefs
        PlayerPrefs.DeleteAll();
        _playerCoins.generateCooldown = _playerCoins.generateMaxCooldown;
        _updateDisplayMain.UpdateDisplayProvince();

        PlayerPrefs.Save();
    }

    public void IncreaseLives()
    {
        int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
        if (globalLives < _playerLives.livesMax)
            // Increment the player life by one
            PlayerPrefs.SetInt("GlobalLives", globalLives + 1);
    }

    public void DecreaseLives()
    {
        int globalLives = PlayerPrefs.GetInt("GlobalLives", _playerLives.livesMax);
        if (globalLives > 0)
            // Decrement the player life by one
            PlayerPrefs.SetInt("GlobalLives", globalLives - 1);
    }

    public void IncreaseLevel()
    {
        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        if (provinceCompleted != provinceUnlocked)
        {
            // Increment the no. of completed province if it is not equal to the no. of unlocked province
            PlayerPrefs.SetInt("ProvinceCompleted", provinceCompleted + 1);
        }
        else if (provinceUnlocked < _playerProvince.provinceTotal)
        {
            // Increment the no. of unlocked province if it is still less than the total no. of province
            PlayerPrefs.SetInt("ProvinceUnlocked", provinceUnlocked + 1);
            PlayerPrefs.SetInt(_levelLoad.primalTravelKeyNames[provinceUnlocked - 1], 1);
        }
        _updateDisplayMain.UpdateDisplayProvince();
    }

    public void DecreaseLevel()
    {
        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        if (provinceCompleted != provinceUnlocked)
        {
            // Decrement the no. of unlocked province if it is not equal to the no. of completed province
            PlayerPrefs.SetInt("ProvinceUnlocked", provinceUnlocked - 1);
            PlayerPrefs.SetInt(_levelLoad.primalTravelKeyNames[provinceUnlocked - 1], 1);
        }
        else if (provinceUnlocked > 0)
        {
            // Decrement the no. of completed province if the no. of unlocked province is more than 0
            PlayerPrefs.SetInt("ProvinceCompleted", provinceCompleted - 1);
        }
        _updateDisplayMain.UpdateDisplayProvince();
    }

    public void IncreaseCoins()
    {
        // Increase coins by 10.0
        _playerCoins.IncreaseCoins(10.0f);
    }

    public void DecreaseCoins()
    {
        // Decrease coins by 10.0
        _playerCoins.DecreaseCoins(10.0f);
    }
}
