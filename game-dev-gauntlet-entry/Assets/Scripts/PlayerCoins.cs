using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCoins : MonoBehaviour
{
    public float globalCoins;
    public Text coinsText;

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
        UpdateDisplay();
    }

    private void Update()
    {
        globalCoins = PlayerPrefs.GetFloat("GlobalCoins", 0);
    }

    public void UpdateDisplay()
    {
        if (SceneManager.GetActiveScene().name == levelLoad.mainScene)
            coinsText.text = string.Format("{0:0.00}", PlayerPrefs.GetFloat("GlobalCoins", 0));
    }

    public void IncreaseCoins(float increase)
    {
        PlayerPrefs.SetFloat("GlobalCoins", PlayerPrefs.GetFloat("GlobalCoins", 0) + increase);
        PlayerPrefs.Save();
    }

    public void DecreaseCoins(float decrease)
    {
        if (PlayerPrefs.GetFloat("GlobalCoins", 0) >= decrease)
        {
            PlayerPrefs.SetFloat("GlobalCoins", PlayerPrefs.GetFloat("GlobalCoins", 0) - decrease);
            PlayerPrefs.Save();
        }
    }
}
