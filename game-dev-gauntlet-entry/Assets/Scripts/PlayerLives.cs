using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    public int globalLives;
    public int failsBeforeSuccess;
    public int livesMax;
    public int lifeMaxCooldown;
    public float lifeCooldown;
    public bool inCooldown = false;
    
    private UpdateDisplayMain _updateDisplayMain;
    private static GameObject s_instance {set; get;}
    
    private void Awake()
    {
        // Will not Destroy the Script When on the Next Scene
        if (s_instance != null) 
            Destroy(s_instance);
        s_instance = gameObject;
        DontDestroyOnLoad(s_instance);

        // Referencing the Scripts from GameObjects
        _updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();

        globalLives = PlayerPrefs.GetInt("GlobalLives", livesMax);
        failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);
        lifeCooldown = PlayerPrefs.GetFloat("LifeCooldown", lifeMaxCooldown);
        _updateDisplayMain.UpdateDisplayLives();
    }

    private void Start()
    {
        OfflineCooldown(PlayerPrefs.GetInt("OffCooldownCheck", 1));
        _updateDisplayMain.UpdateDisplayLives();
    }

    private void OfflineCooldown(int offline)
    {
        // Decrease Life Cooldown or Increase Lives based on the Offline Time
        if (offline == 1)
        {
            PlayerPrefs.SetInt("OffCooldownCheck", 0);
            DateTime timeCurrent = DateTime.Now;
            if (PlayerPrefs.HasKey("SavedTime"))
            {
                DateTime timeSaved = DateTime.Parse(PlayerPrefs.GetString("SavedTime"));
                TimeSpan timePassed = timeCurrent - timeSaved;
                float timeLeftFromOffline = (float)timePassed.TotalSeconds;
                
                while (timeLeftFromOffline > 0)
                {
                    int globalLivesCurrent = PlayerPrefs.GetInt("GlobalLives", livesMax);
                    float lifeCooldownCurrent = PlayerPrefs.GetFloat("LifeCooldown", lifeMaxCooldown);

                    if (globalLivesCurrent >= livesMax)
                        timeLeftFromOffline = 0;

                    if (timeLeftFromOffline > lifeCooldownCurrent)
                    {
                        timeLeftFromOffline -= lifeCooldownCurrent;
                        PlayerPrefs.SetInt("GlobalLives", globalLivesCurrent + 1);
                        PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
                        lifeCooldown = 0;
                        _updateDisplayMain.UpdateDisplayLives();
                    }
                    else
                    {
                        PlayerPrefs.SetFloat("LifeCooldown", lifeCooldownCurrent - timeLeftFromOffline);
                        lifeCooldown = lifeCooldownCurrent - timeLeftFromOffline;
                        timeLeftFromOffline = 0;
                    }
                }
            }
        }
    }

    public void RewardLife(int discount, bool recentRound)
    {
        // Decrease Life Cooldown based on Fails Before Success
        if (discount > 0 && recentRound)
        {
            float timeLeftFromDiscount = lifeMaxCooldown / (discount * 2);
            PlayerPrefs.SetInt("FailsBeforeSuccess", 0);

            while (timeLeftFromDiscount > 0)
            {
                int globalLivesCurrent = PlayerPrefs.GetInt("GlobalLives", livesMax);
                float lifeCooldownCurrent = PlayerPrefs.GetFloat("LifeCooldown", lifeMaxCooldown);
                
                if (globalLivesCurrent >= livesMax)
                    timeLeftFromDiscount = 0;

                if (timeLeftFromDiscount > lifeCooldownCurrent)
                {
                    timeLeftFromDiscount -= lifeCooldownCurrent;
                    PlayerPrefs.SetInt("GlobalLives", globalLivesCurrent + 1);
                    PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
                    lifeCooldown = 0;
                    _updateDisplayMain.UpdateDisplayLives();
                }
                else
                {
                    PlayerPrefs.SetFloat("LifeCooldown", lifeCooldownCurrent - timeLeftFromDiscount);
                    lifeCooldown = lifeCooldownCurrent - timeLeftFromDiscount;
                    timeLeftFromDiscount = 0;
                }
            }
        }
    }

    private void Update()
    {
        globalLives = PlayerPrefs.GetInt("GlobalLives", livesMax);
        failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);

        // Life Cooldown
        if (globalLives < livesMax)
        {
            if (!inCooldown)
            {
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
                    PlayerPrefs.SetInt("GlobalLives", globalLives + 1);
                    PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
                    lifeCooldown = lifeMaxCooldown;
                    inCooldown = false;
                    _updateDisplayMain.UpdateDisplayLives();
                }

                if (Mathf.FloorToInt(lifeCooldown % 1) == 0)
                {
                    PlayerPrefs.SetFloat("LifeCooldown", Mathf.FloorToInt(lifeCooldown));
                    _updateDisplayMain.UpdateDisplayLives();
                }
            }
        }
        else
        {
            PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
            lifeCooldown = lifeMaxCooldown;
            inCooldown = false;
            _updateDisplayMain.UpdateDisplayLives();
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("SavedTime", DateTime.Now.ToString());
        PlayerPrefs.SetInt("OffCooldownCheck", 1);
        PlayerPrefs.SetInt("FailsBeforeSuccess", 0);
        PlayerPrefs.Save();
    }
}
