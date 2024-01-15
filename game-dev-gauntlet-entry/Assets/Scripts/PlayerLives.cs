using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour
{
    public int globalLives;
    public int livesMax;
    public int lifeMaxCooldown;
    private float lifeCooldown;
    private bool inCooldown = false;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Image livesImage;
    public Text livesText;
    public Text livesCooldownText;
    
    private LevelLoad levelLoad;
    private static GameObject sampleInstance;
    private void Awake()
    {
        if (sampleInstance != null) 
            Destroy(sampleInstance);

        sampleInstance = gameObject;
        DontDestroyOnLoad(sampleInstance);

        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
    }

    private void Start()
    {
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
                
                while (timeLeftFromOffline > 0)
                {
                    if (PlayerPrefs.GetInt("GlobalLives", livesMax) >= livesMax)
                        timeLeftFromOffline = 0;

                    if (timeLeftFromOffline > PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown))
                    {
                        timeLeftFromOffline -= PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown);
                        PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", livesMax) + 1);
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
                if (PlayerPrefs.GetInt("GlobalLives", livesMax) >= livesMax)
                    timeLeftFromDiscount = 0;

                if (timeLeftFromDiscount > PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown))
                {
                    timeLeftFromDiscount -= PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown);
                    PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", livesMax) + 1);
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
        globalLives = PlayerPrefs.GetInt("GlobalLives", livesMax);
        if (PlayerPrefs.GetInt("GlobalLives", livesMax) < livesMax)
        {
            if (!inCooldown)
            {
                lifeCooldown = PlayerPrefs.GetInt("LifeCooldown", lifeMaxCooldown);
                inCooldown = true;
            }
            else
            {
                if (lifeCooldown > 0)
                    lifeCooldown -= Time.deltaTime;
                else if (lifeCooldown <= 0)
                {
                    PlayerPrefs.SetInt("GlobalLives", PlayerPrefs.GetInt("GlobalLives", livesMax) + 1);
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
        else if (PlayerPrefs.GetInt("GlobalLives", livesMax) == livesMax)
        {
            UpdateDisplay();
            PlayerPrefs.SetInt("LifeCooldown", lifeMaxCooldown);
            PlayerPrefs.Save();
            inCooldown = false;
        }
    }

    public void UpdateDisplay()
    {
        if (SceneManager.GetActiveScene().name == levelLoad.mainScene)
        {
            livesText.text = PlayerPrefs.GetInt("GlobalLives", livesMax).ToString();
            if (PlayerPrefs.GetInt("GlobalLives", livesMax) < livesMax)
            {
                if (inCooldown)
                {
                    int minutes = Mathf.FloorToInt(lifeCooldown / 60);
                    int seconds = Mathf.FloorToInt(lifeCooldown % 60);
                    livesCooldownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                }
                if (PlayerPrefs.GetInt("GlobalLives", livesMax) == 0)
                    livesImage.sprite = emptyHeart;
                else
                    livesImage.sprite = halfHeart;
            } 
            else if (PlayerPrefs.GetInt("GlobalLives", livesMax) == livesMax)
            {
                livesCooldownText.text = "FULL";
                livesImage.sprite = fullHeart;
            }
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("SavedTime", DateTime.Now.ToString());
        PlayerPrefs.SetInt("OffCooldownCheck", 0);
        PlayerPrefs.SetInt("FailsBeforeWin", 0);
        PlayerPrefs.Save();
    }
}
