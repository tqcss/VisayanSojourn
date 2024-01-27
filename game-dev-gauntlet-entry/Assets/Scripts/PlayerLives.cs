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
    
    private LevelLoad levelLoad;
    private UpdateDisplayMain updateDisplayMain;
    private static GameObject instancePlayerLives {set; get;}
    private void Awake()
    {
        if (instancePlayerLives != null) 
            Destroy(instancePlayerLives);

        instancePlayerLives = gameObject;
        DontDestroyOnLoad(instancePlayerLives);

        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();

        globalLives = PlayerPrefs.GetInt("GlobalLives", livesMax);
        failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);
        lifeCooldown = PlayerPrefs.GetFloat("LifeCooldown", lifeMaxCooldown);
        updateDisplayMain.UpdateDisplayLives();
    }

    private void Start()
    {
        OfflineCooldown(PlayerPrefs.GetInt("OffCooldownCheck", 1));
        RewardLife(PlayerPrefs.GetInt("FailsBeforeSuccess", 0));
        updateDisplayMain.UpdateDisplayLives();
    }

    private void OfflineCooldown(int offline)
    {
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
                    globalLives = PlayerPrefs.GetInt("GlobalLives", livesMax);
                    lifeCooldown = PlayerPrefs.GetFloat("LifeCooldown", lifeMaxCooldown);

                    if (globalLives >= livesMax)
                        timeLeftFromOffline = 0;

                    if (timeLeftFromOffline > lifeCooldown)
                    {
                        timeLeftFromOffline -= lifeCooldown;
                        PlayerPrefs.SetInt("GlobalLives", globalLives + 1);
                        PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
                        updateDisplayMain.UpdateDisplayLives();
                    }
                    else
                    {
                        PlayerPrefs.SetFloat("LifeCooldown", lifeCooldown - timeLeftFromOffline);
                        timeLeftFromOffline = 0;
                    }
                }
            }
        }
    }

    public void RewardLife(int discount)
    {
        if (discount > 0)
        {
            float timeLeftFromDiscount = lifeMaxCooldown / (discount * 2);
            PlayerPrefs.SetInt("FailsBeforeSuccess", 0);

            while (timeLeftFromDiscount > 0)
            {
                globalLives = PlayerPrefs.GetInt("GlobalLives", livesMax);
                lifeCooldown = PlayerPrefs.GetFloat("LifeCooldown", lifeMaxCooldown);
                
                if (globalLives >= livesMax)
                    timeLeftFromDiscount = 0;

                if (timeLeftFromDiscount > lifeCooldown)
                {
                    timeLeftFromDiscount -= lifeCooldown;
                    PlayerPrefs.SetInt("GlobalLives", globalLives + 1);
                    PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
                    updateDisplayMain.UpdateDisplayLives();
                }
                else
                {
                    PlayerPrefs.SetFloat("LifeCooldown", lifeCooldown - timeLeftFromDiscount);
                    timeLeftFromDiscount = 0;
                }
            }
        }
    }

    private void Update()
    {
        globalLives = PlayerPrefs.GetInt("GlobalLives", livesMax);
        failsBeforeSuccess = PlayerPrefs.GetInt("FailsBeforeSuccess", 0);

        if (globalLives < livesMax)
        {
            if (!inCooldown)
            {
                lifeCooldown = PlayerPrefs.GetFloat("LifeCooldown", lifeMaxCooldown);
                inCooldown = true;
            }
            else
            {
                if (lifeCooldown > 0)
                    lifeCooldown -= Time.deltaTime;
                else if (lifeCooldown <= 0)
                {
                    PlayerPrefs.SetInt("GlobalLives", globalLives + 1);
                    PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
                    inCooldown = false;
                    updateDisplayMain.UpdateDisplayLives();
                }

                if (Mathf.FloorToInt(lifeCooldown % 1) == 0)
                {
                    PlayerPrefs.SetFloat("LifeCooldown", Mathf.FloorToInt(lifeCooldown));
                    updateDisplayMain.UpdateDisplayLives();
                }
            }
        }
        else if (globalLives == livesMax)
        {
            PlayerPrefs.SetFloat("LifeCooldown", lifeMaxCooldown);
            updateDisplayMain.UpdateDisplayLives();
            inCooldown = false;
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
