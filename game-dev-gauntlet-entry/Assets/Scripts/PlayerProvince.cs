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
    public float[] provinceCost;
    public string[] recipeDoneKeyName = {"RecipeDoneAntique", "RecipeDoneAklan", "RecipeDoneCapiz", "RecipeDoneNegrosOcc", "RecipeDoneGuimaras", "RecipeDoneIloilo"};
    public int[] recipeDoneValue;

    private LevelLoad levelLoad;
    private PlayerCoins playerCoins;
    private UpdateDisplayMain updateDisplayMain;
    private static GameObject instancePlayerProvince {set; get;}
    private void Awake()
    {
        if (instancePlayerProvince != null) 
            Destroy(instancePlayerProvince);

        instancePlayerProvince = gameObject;
        DontDestroyOnLoad(instancePlayerProvince);
        
        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        playerCoins = GameObject.FindGameObjectWithTag("playerCoins").GetComponent<PlayerCoins>();
        updateDisplayMain = GameObject.FindGameObjectWithTag("mainScript").GetComponent<UpdateDisplayMain>();
    }

    private void Update()
    {
        provinceCurrent = PlayerPrefs.GetInt("ProvinceCurrent", 0);
        provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        for (int i = 0; i < recipeDoneKeyName.Length; i++)
            recipeDoneValue[i] = PlayerPrefs.GetInt(recipeDoneKeyName[i], 1);
        updateDisplayMain.UpdateDisplayProvince();
    }

    public void ProvincePurchasing()
    {
        int provinceCompleted = PlayerPrefs.GetInt("ProvinceCompleted", 0);
        if (PlayerPrefs.GetFloat("GlobalCoins", 0) >= provinceCost[provinceCompleted])
        {
            playerCoins.DecreaseCoins(provinceCost[provinceCompleted]);
            PlayerPrefs.SetInt("ProvinceUnlocked", PlayerPrefs.GetInt("ProvinceUnlocked", 1) + 1);
            levelLoad.PlayAnimation();
        }
    }
}
