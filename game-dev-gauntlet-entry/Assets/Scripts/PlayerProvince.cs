using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProvince : MonoBehaviour
{
    
    public int provinceTotal;
    private int provinceUnlocked;

    public Image[] locationMarker;
    public Button[] locationButton;
    public GameObject[] locationButtonObj;
    public GameObject[] mapImage;
    public Sprite unlockLocation;
    public Sprite lockLocation;
    public Button backDescButton;

    private void Awake()
    {
        provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        UpdateProvince();
    }

    private void UpdateProvince()
    {
        for (int i = 0; i < locationMarker.Length; i++)
        {
            locationMarker[i].sprite = lockLocation;
            locationButton[i].interactable = false;
            locationButtonObj[i].SetActive(false);
            mapImage[i].SetActive(false);
        }
        for (int i = 0; i < PlayerPrefs.GetInt("ProvinceUnlocked", 1); i++)
        {
            if (i < provinceTotal)
            {
                locationMarker[i].sprite = unlockLocation;
                locationButton[i].interactable = true;
                locationButtonObj[i].SetActive(true);
                mapImage[i].SetActive(true);
            }    
        }
    }

    public void DisableProvince()
    {
        backDescButton.interactable = true;
        for (int j = 0; j < locationMarker.Length; j++)
        {
            locationButton[j].interactable = false;
        }
    }

    public void EnableProvince()
    {
        backDescButton.interactable = false;
        StartCoroutine(DelayEnable());
    }

    private IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(1);
        for (int j = 0; j < PlayerPrefs.GetInt("ProvinceUnlocked", 1); j++)
        {
            if (j < provinceTotal)
            {
                locationButton[j].interactable = true;
            }
        }
    }

}
