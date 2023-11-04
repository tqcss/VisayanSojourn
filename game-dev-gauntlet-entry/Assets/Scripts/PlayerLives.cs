using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerLives : MonoBehaviour
{
    
    public int livesTotal;
    private int livesGlobal;
    public int lifeMaxCooldown;
    private float lifeCooldown;
    private bool inCooldown = false;

    public Image[] LifeSet;
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    public TextMeshProUGUI TimerCDText;
    
    private static GameObject sampleInstance;
    private void Awake()
    {
        if (sampleInstance != null)
        {
            Destroy(sampleInstance);
        }
        sampleInstance = gameObject;
        DontDestroyOnLoad(this);
        
        livesGlobal = PlayerPrefs.GetInt("GlobalLives", livesTotal);
        lifeCooldown = PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown);
        UpdateLives();

        OfflineCooldown(PlayerPrefs.GetInt("OffCooldownCheck", 0));
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
                int timeLeftSub = (int)(timePassed.TotalSeconds);
                Debug.Log(timeLeftSub);
                
                while (timeLeftSub > 0)
                {
                    if (PlayerPrefs.GetInt("GlobalLives", livesTotal) >= livesTotal)
                    {
                        timeLeftSub = 0;
                    }

                    if (timeLeftSub > PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown))
                    {
                        timeLeftSub -= PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown);
                        PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", livesTotal) + 1);
                        PlayerPrefs.SetInt("LifeCooldown", lifeMaxCooldown);
                        PlayerPrefs.Save();
                        UpdateLives();
                    }
                    else
                    {
                        PlayerPrefs.SetInt("LifeCooldown", PlayerPrefs.GetInt("LifeCooldown") - timeLeftSub);
                        PlayerPrefs.Save();
                        timeLeftSub = 0;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("GlobalLives", livesTotal) < livesTotal)
        {
            if (inCooldown == false)
            {
                lifeCooldown = PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown);
                inCooldown = true;
            }
        }

        if (inCooldown == true)
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
                UpdateLives();
                inCooldown = false;
            }
            
            if (Mathf.FloorToInt(lifeCooldown % 1) == 0)
            {
                PlayerPrefs.SetInt("LifeCooldown", Mathf.FloorToInt(lifeCooldown));
                PlayerPrefs.Save();
            }
            
            int minutes = Mathf.FloorToInt(lifeCooldown / 60);
            int seconds = Mathf.FloorToInt(lifeCooldown % 60);
            TimerCDText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (PlayerPrefs.GetInt("GlobalLives", livesTotal) == livesTotal)
        {
            TimerCDText.text = "";
            PlayerPrefs.SetInt("LifeCooldown", lifeMaxCooldown);
            PlayerPrefs.Save();
            inCooldown = false;
        }
    }

    public void UpdateLives()
    {
        for (int i = 0; i < LifeSet.Length; i++)
        {
            LifeSet[i].sprite = EmptyHeart;
        }
        for (int i = 0; i < PlayerPrefs.GetInt("GlobalLives", livesTotal); i++)
        {
            if (i < livesTotal)
            {
                LifeSet[i].sprite = FullHeart;
            }    
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("SavedTime", DateTime.Now.ToString());
        PlayerPrefs.SetInt("OffCooldownCheck", 0);
    }

}
