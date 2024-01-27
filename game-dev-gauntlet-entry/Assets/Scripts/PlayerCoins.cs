using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCoins : MonoBehaviour
{
    public float globalCoins;

    private LevelLoad levelLoad;
    private UpdateDisplayMain updateDisplayMain;
    private static GameObject instancePlayerCoins {set; get;}
    private void Awake()
    {
        if (instancePlayerCoins != null) 
            Destroy(instancePlayerCoins);

        instancePlayerCoins = gameObject;
        DontDestroyOnLoad(instancePlayerCoins);

        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();
    }

    private void Update()
    {
        globalCoins = PlayerPrefs.GetFloat("GlobalCoins", 0);
        updateDisplayMain.UpdateDisplayCoins();
    }

    public void IncreaseCoins(float increase)
    {
        PlayerPrefs.SetFloat("GlobalCoins", PlayerPrefs.GetFloat("GlobalCoins", 0) + increase);
    }

    public void DecreaseCoins(float decrease)
    {
        if (PlayerPrefs.GetFloat("GlobalCoins", 0) >= decrease)
            PlayerPrefs.SetFloat("GlobalCoins", PlayerPrefs.GetFloat("GlobalCoins", 0) - decrease);
        else
            PlayerPrefs.SetFloat("GlobalCoins", 0);
    }
}
