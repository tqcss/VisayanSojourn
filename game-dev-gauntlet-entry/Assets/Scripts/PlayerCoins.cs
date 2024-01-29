using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCoins : MonoBehaviour
{
    public float globalCoins;

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
    }

    private void Start()
    {
        _updateDisplayMain.UpdateDisplayCoins();
    }

    private void Update()
    {
        globalCoins = PlayerPrefs.GetFloat("GlobalCoins", 0);
        _updateDisplayMain.UpdateDisplayCoins();
    }

    public void IncreaseCoins(float increase)
    {
        // Increase Coins by Amount Prompted
        PlayerPrefs.SetFloat("GlobalCoins", PlayerPrefs.GetFloat("GlobalCoins", 0) + increase);
    }

    public void DecreaseCoins(float decrease)
    {
        // Decrease Coins by Amount Prompted
        if (PlayerPrefs.GetFloat("GlobalCoins", 0) >= decrease)
            PlayerPrefs.SetFloat("GlobalCoins", PlayerPrefs.GetFloat("GlobalCoins", 0) - decrease);
        else
            PlayerPrefs.SetFloat("GlobalCoins", 0);
    }
}
