using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerProvince : MonoBehaviour
{
    public int provinceCurrent;
    public int provinceCompleted;
    public int provinceUnlocked;
    public int provinceTotal;
    public float returnPercentageCost;
    public float[] provinceCost;
    public string[] recipeDoneKeyName = {"RecipeDoneAntique", "RecipeDoneAklan", "RecipeDoneCapiz", "RecipeDoneNegrosOcc", "RecipeDoneGuimaras", "RecipeDoneIloilo"};
    public int[] recipeDoneValue;

    private LevelLoad _levelLoad;
    private PlayerCoins _playerCoins;
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
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        _playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        _updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();
    }

    private void Start()
    {
        _updateDisplayMain.UpdateDisplayProvince();
    }

    private void Update()
    {
        // Automatically update the player province
        provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        for (int i = 0; i < recipeDoneKeyName.Length; i++)
            recipeDoneValue[i] = PlayerPrefs.GetInt(recipeDoneKeyName[i], 1);
        _updateDisplayMain.UpdateDisplayProvince();
    }

    public void ProvincePurchasing()
    {
        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);

        // Check if the player has more coins than the cost of locked province
        if (PlayerPrefs.GetFloat("GlobalCoins", _playerCoins.initialCoins) >= provinceCost[provinceCompleted])
        {
            // Decrease the player global coin by the cost of a purchased province and grant access to it
            _playerCoins.DecreaseCoins(provinceCost[provinceCompleted]);

            // Grant player return percentage of the province cost as a service of buying
            float returnCoins = provinceCost[provinceCompleted] * (returnPercentageCost / 100.0f);
            _playerCoins.IncreaseCoins(returnCoins);

            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            _levelLoad.PlayAnimation();
        }
    }
}
