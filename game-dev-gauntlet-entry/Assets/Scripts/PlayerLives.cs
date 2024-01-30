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
        // Will not destroy the script when on the next loaded scene
        if (s_instance != null) 
            Destroy(s_instance);
        s_instance = gameObject;
        DontDestroyOnLoad(s_instance);

        // Reference the scripts from game objects
        _updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();

        // Set initial values to the variables 
        globalLives = PlayerPrefs.GetInt("GlobalLives", livesMax);
        failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);
        lifeCooldown = PlayerPrefs.GetFloat("LifeCooldown", lifeMaxCooldown);
        _updateDisplayMain.UpdateDisplayLives();
    }

    private void Start()
    {
        OfflineCooldown(PlayerPrefs.GetInt("CheckOfflineCooldown", 1));
        _updateDisplayMain.UpdateDisplayLives();
    }

    private void OfflineCooldown(int offline)
    {
        // Check if the user was offline
        if (offline == 1)
        {
            PlayerPrefs.SetInt("CheckOfflineCooldown", 0);
            // Get the current time
            DateTime timeCurrent = DateTime.Now;
            if (PlayerPrefs.HasKey("SavedTime"))
            {
                // Get the time saved after the user quitted from the previous session
                DateTime timeSaved = DateTime.Parse(PlayerPrefs.GetString("SavedTime"));
                // Compute the amount of time the user is offline
                TimeSpan timePassed = timeCurrent - timeSaved;
                float timeLeftFromOffline = (float)timePassed.TotalSeconds;
                
                // Decrease life cooldown or increase lives based on the amount of time the user is offline
                while (timeLeftFromOffline > 0)
                {
                    int globalLivesCurrent = PlayerPrefs.GetInt("GlobalLives", livesMax);
                    float lifeCooldownCurrent = PlayerPrefs.GetFloat("LifeCooldown", lifeMaxCooldown);

                    // Set time left from offline to 0 if the player global life at maximum 
                    if (globalLivesCurrent >= livesMax)
                        timeLeftFromOffline = 0;

                    // Decrease the time left from offline by the current life cooldown
                    // and increment player global life by one if there are more time left from offline than life cooldown
                    if (timeLeftFromOffline > lifeCooldownCurrent)
                    {
                        timeLeftFromOffline -= lifeCooldownCurrent;
                        PlayerPrefs.SetInt("GlobalLives", globalLivesCurrent + 1);
                        PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
                        lifeCooldown = 0;
                        _updateDisplayMain.UpdateDisplayLives();
                    }
                    // Decrease the current life cooldown by the time left from offline
                    // if there are more life cooldown than time left from offline
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

    public void RewardLife(int discount, bool latestRound)
    {
        // Check if the user has fails before success and it is the latest round
        if (discount > 0 && latestRound)
        {
            // Compute the amount of time for decreasing the life cooldown based on the discount
            float timeLeftFromDiscount = lifeMaxCooldown / (discount * 2);
            PlayerPrefs.SetInt("FailsBeforeSuccess", 0);

            // Decrease life cooldown based on fails before success
            while (timeLeftFromDiscount > 0)
            {
                int globalLivesCurrent = PlayerPrefs.GetInt("GlobalLives", livesMax);
                float lifeCooldownCurrent = PlayerPrefs.GetFloat("LifeCooldown", lifeMaxCooldown);
                
                // Set time left from discount to 0 if the player global life at maximum 
                if (globalLivesCurrent >= livesMax)
                    timeLeftFromDiscount = 0;

                // Decrease the time left from discount by the current life cooldown
                // and increment player global life by one if there are more time left from discount than life cooldown
                if (timeLeftFromDiscount > lifeCooldownCurrent)
                {
                    timeLeftFromDiscount -= lifeCooldownCurrent;
                    PlayerPrefs.SetInt("GlobalLives", globalLivesCurrent + 1);
                    PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
                    lifeCooldown = 0;
                    _updateDisplayMain.UpdateDisplayLives();
                }
                // Decrease the current life cooldown by the time left from discount
                // if there are more life cooldown than time left from discount
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
        // Automatically update the player global life
        globalLives = PlayerPrefs.GetInt("GlobalLives", livesMax);
        failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);

        // Check if the player has less life than the maximum
        if (globalLives < livesMax)
        {
            if (!inCooldown)
            {
                // Activate the cooldown if it is inactive
                inCooldown = true;
            }
            else
            {
                // Decrease the life cooldown if it is more than 0
                if (lifeCooldown > 0)
                {
                    lifeCooldown -= Time.deltaTime;
                }
                // Increment player global life by one and reset the life cooldown if it reaches 0
                else if (lifeCooldown <= 0)
                {
                    PlayerPrefs.SetInt("GlobalLives", globalLives + 1);
                    PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
                    lifeCooldown = lifeMaxCooldown;
                    inCooldown = false;
                    _updateDisplayMain.UpdateDisplayLives();
                }

                // Set the life cooldown to its floor
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
        // Save the current time, check if offline, and reset fails before success upon quitting
        PlayerPrefs.SetString("SavedTime", DateTime.Now.ToString());
        PlayerPrefs.SetInt("CheckOfflineCooldown", 1);
        PlayerPrefs.SetInt("FailsBeforeSuccess", 0);
        PlayerPrefs.Save();
    }
}
