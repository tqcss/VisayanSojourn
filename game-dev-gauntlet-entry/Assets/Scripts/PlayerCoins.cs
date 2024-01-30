using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCoins : MonoBehaviour
{
    public float globalCoins;
    public float initialCoins;

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
    }

    private void Start()
    {
        _updateDisplayMain.UpdateDisplayCoins();
    }

    private void Update()
    {
        // Automatically update the player global coins
        globalCoins = PlayerPrefs.GetFloat("GlobalCoins", initialCoins);
        _updateDisplayMain.UpdateDisplayCoins();
    }

    public void IncreaseCoins(float increase)
    {
        // Increase coins by the amount prompted
        float globalCoins = PlayerPrefs.GetFloat("GlobalCoins", initialCoins);
        PlayerPrefs.SetFloat("GlobalCoins", globalCoins + increase);
    }

    public void DecreaseCoins(float decrease)
    {
        // Decrease coins by the amount prompted
        float globalCoins = PlayerPrefs.GetFloat("GlobalCoins", initialCoins);
        PlayerPrefs.SetFloat("GlobalCoins", (globalCoins >= decrease) ? globalCoins - decrease : 0);
    }
}
