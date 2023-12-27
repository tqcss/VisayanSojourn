using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin : MonoBehaviour
{
    private PlayerLives playerLives;
    private PlayerProvince playerProvince;
    
    private void Awake()
    {
        playerLives = GameObject.FindGameObjectWithTag("playerLives").GetComponent<PlayerLives>();
        playerProvince = GameObject.FindGameObjectWithTag("mainScript").GetComponent<PlayerProvince>();
    }

    public void DeleteSavedData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void IncreaseLives()
    {
        if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) < playerLives.livesTotal)
        {
            PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) + 1);
            PlayerPrefs.Save();
        }
    }

    public void DecreaseLives()
    {
        if (PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) > 0)
        {
            PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", playerLives.livesTotal) - 1);
            PlayerPrefs.Save();
        }
    }

    public void IncreaseLevel()
    {
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) < playerProvince.provinceTotal)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            PlayerPrefs.Save();
            playerProvince.UpdateProvince();
        }
    }

    public void DecreaseLevel()
    {
        if (PlayerPrefs.GetInt("ProvinceUnlocked", 1) > 1)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) - 1);
            PlayerPrefs.Save();
            playerProvince.UpdateProvince();
        }
    }
}
