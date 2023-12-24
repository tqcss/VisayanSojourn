using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    
    public int livesTotal;
    private int livesGlobal;
    public int lifeMaxCooldown;
    private float lifeCooldown;
    private bool inCooldown = false;

    public Image[] lifeSet;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Text timerCDText;
    
    private static GameObject sampleInstance;
    private void Start()
    {
        if (sampleInstance != null)
        {
            Destroy(sampleInstance);
        }
        sampleInstance = gameObject;
        DontDestroyOnLoad(sampleInstance);
        
        livesGlobal = PlayerPrefs.GetInt("GlobalLives", livesTotal);
        lifeCooldown = PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown);

        OfflineCooldown(PlayerPrefs.GetInt("OffCooldownCheck", 0));
        RewardLife(PlayerPrefs.GetInt("FailsBeforeWin", 0));
        UpdateDisplay();
    }

    private void OfflineCooldown(int offline)
    {
        if (offline == 0)
        {
            PlayerPrefs.SetInt("OffCooldownCheck", 1);
            DateTime timeCurrent = DateTime.Now;
            if (PlayerPrefs.HasKey("SavedTime"))
            {
                DateTime timeSaved = DateTime.Parse(PlayerPrefs.GetString("SavedTime"));
                TimeSpan timePassed = timeCurrent - timeSaved;
                int timeLeftFromOffline = (int)(timePassed.TotalSeconds);
                Debug.Log(timeLeftFromOffline);
                
                while (timeLeftFromOffline > 0)
                {
                    if (PlayerPrefs.GetInt("GlobalLives", livesTotal) >= livesTotal)
                    {
                        timeLeftFromOffline = 0;
                    }

                    if (timeLeftFromOffline > PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown))
                    {
                        timeLeftFromOffline -= PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown);
                        PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", livesTotal) + 1);
                        PlayerPrefs.SetInt("LifeCooldown", lifeMaxCooldown);
                        PlayerPrefs.Save();
                        UpdateDisplay();
                    }
                    else
                    {
                        PlayerPrefs.SetInt("LifeCooldown", PlayerPrefs.GetInt("LifeCooldown") - timeLeftFromOffline);
                        PlayerPrefs.Save();
                        timeLeftFromOffline = 0;
                    }
                }
            }
        }
    }

    private void RewardLife(int discount)
    {
        if (discount > 0)
        {
            int timeLeftFromDiscount = lifeMaxCooldown / (discount * 2);
            PlayerPrefs.SetInt("FailsBeforeWin", 0);
            while (timeLeftFromDiscount > 0)
            {
                if (PlayerPrefs.GetInt("GlobalLives", livesTotal) >= livesTotal)
                {
                    timeLeftFromDiscount = 0;
                }

                if (timeLeftFromDiscount > PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown))
                {
                    timeLeftFromDiscount -= PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown);
                    PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", livesTotal) + 1);
                    PlayerPrefs.SetInt("LifeCooldown", lifeMaxCooldown);
                    PlayerPrefs.Save();
                    UpdateDisplay();
                }
                else
                {
                    PlayerPrefs.SetInt("LifeCooldown", PlayerPrefs.GetInt("LifeCooldown") - timeLeftFromDiscount);
                    PlayerPrefs.Save();
                    timeLeftFromDiscount = 0;
                }
            }
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("GlobalLives", livesTotal) < livesTotal)
        {
            if (!inCooldown)
            {
                lifeCooldown = PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown);
                inCooldown = true;
            }
            else
            {
                if (lifeCooldown > 0)
                {
                    lifeCooldown -= Time.deltaTime;
                }
                else if (lifeCooldown <= 0)
                {
                    PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", livesTotal) + 1);
                    PlayerPrefs.SetInt("LifeCooldown", lifeMaxCooldown);
                    PlayerPrefs.Save();
                    inCooldown = false;
                    UpdateDisplay();
                }

                if (Mathf.FloorToInt(lifeCooldown % 1) == 0)
                {
                    PlayerPrefs.SetInt("LifeCooldown", Mathf.FloorToInt(lifeCooldown));
                    PlayerPrefs.Save();
                    UpdateDisplay();
                }
            }
        }
        else if (PlayerPrefs.GetInt("GlobalLives", livesTotal) == livesTotal)
        {
            UpdateDisplay();
            PlayerPrefs.SetInt("LifeCooldown", lifeMaxCooldown);
            PlayerPrefs.Save();
            inCooldown = false;
        }
    }

    public void UpdateDisplay()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            for (int i = 0; i < lifeSet.Length; i++)
            {
                lifeSet[i].sprite = emptyHeart;
            }
            for (int i = 0; i < PlayerPrefs.GetInt("GlobalLives", livesTotal); i++)
            {
                if (i < livesTotal)
                {
                    lifeSet[i].sprite = fullHeart;
                }    
            }
            if ((lifeCooldown < lifeMaxCooldown / 2) && ((PlayerPrefs.GetInt("GlobalLives", livesTotal) - 1) < (livesTotal - 1)))
            {
                lifeSet[(PlayerPrefs.GetInt("GlobalLives", livesTotal))].sprite = halfHeart;
            }

            if (PlayerPrefs.GetInt("GlobalLives", livesTotal) < livesTotal)
            {
                if (inCooldown)
                {
                    int minutes = Mathf.FloorToInt(lifeCooldown / 60);
                    int seconds = Mathf.FloorToInt(lifeCooldown % 60);
                    timerCDText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                }
            } 
            else if (PlayerPrefs.GetInt("GlobalLives", livesTotal) == livesTotal)
            {
                timerCDText.text = "";
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("SavedTime", DateTime.Now.ToString());
        PlayerPrefs.SetInt("OffCooldownCheck", 0);
        PlayerPrefs.SetInt("FailsBeforeWin", 0);
    }

}
