using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOnLevel : MonoBehaviour
{
    
    public int livesTotal;
    public void FinishedAntique (bool finished)
    {
        if ((PlayerPrefs.GetInt("GlobalLives", livesTotal) > 0) && PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 1)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void FinishedAklan (bool finished)
    {
        if ((PlayerPrefs.GetInt("GlobalLives", livesTotal) > 0) && PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 2)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void FinishedCapiz (bool finished)
    {
        if ((PlayerPrefs.GetInt("GlobalLives", livesTotal) > 0) && PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 3)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void FinishedNegrosOcc (bool finished)
    {
        if ((PlayerPrefs.GetInt("GlobalLives", livesTotal) > 0) && PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 4)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void FinishedGuimaras (bool finished)
    {
        if ((PlayerPrefs.GetInt("GlobalLives", livesTotal) > 0) && PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 5)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void FinishedIloilo (bool finished)
    {
        if ((PlayerPrefs.GetInt("GlobalLives", livesTotal) > 0) && PlayerPrefs.GetInt("ProvinceUnlocked", 1) == 6)
        {
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    public void FailedLevel (bool failed)
    {
        if ((PlayerPrefs.GetInt("GlobalLives", livesTotal) > 0))
        {
            PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", livesTotal) - 1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("No more lives.");
        }
    }

}
