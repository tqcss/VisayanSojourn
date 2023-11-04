using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin : MonoBehaviour
{
    
    public int livesTotal;
    public int provinceTotal;
    public void DeleteSavedData ()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void IncreaseLives ()
    {
        if ((PlayerPrefs.GetInt("GlobalLives", livesTotal) < livesTotal))
        {
            PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", livesTotal) + 1);
            PlayerPrefs.Save();
        }
    }

    public void DecreaseLives ()
    {
        if ((PlayerPrefs.GetInt("GlobalLives", livesTotal) > 0))
        {
            PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", livesTotal) - 1);
            PlayerPrefs.Save();
        }
    }

    public void IncreaseLevel ()
    {
        if ((PlayerPrefs.GetInt("ProvinceUnlocked", 1) < provinceTotal))
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void DecreaseLevel ()
    {
        if ((PlayerPrefs.GetInt("ProvinceUnlocked", 1) > 1))
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) - 1);
            PlayerPrefs.Save();
        }
    }

}
