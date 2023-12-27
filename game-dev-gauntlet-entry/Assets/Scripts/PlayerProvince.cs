using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProvince : MonoBehaviour
{
    public int provinceTotal;
    public Image[] locationMarker;
    public Button[] locationButton;
    public GameObject[] locationButtonObj;
    public Image mapImage;
    public Sprite[] mapSprite;
    public Sprite unlockLocation;
    public Sprite lockLocation;
    public Button backDescButton;

    private void Awake()
    {
        UpdateProvince();
    }

    public void UpdateProvince()
    {
        for (int i = 0; i < locationMarker.Length; i++)
        {
            locationMarker[i].sprite = lockLocation;
            locationButton[i].interactable = false;
            locationButtonObj[i].SetActive(false);
        }
        for (int i = 0; i < PlayerPrefs.GetInt("ProvinceUnlocked", 1); i++)
        {
            if (i < locationMarker.Length)
            {
                locationMarker[i].sprite = unlockLocation;
                locationButton[i].interactable = true;
                locationButtonObj[i].SetActive(true);
            }    
        }
        mapImage.sprite = mapSprite[PlayerPrefs.GetInt("ProvinceUnlocked", 1) - 1];
    }

    public void DisableProvince()
    {
        backDescButton.interactable = true;
        for (int j = 0; j < locationMarker.Length; j++)
            locationButton[j].interactable = false;
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
            if (j < locationMarker.Length)
                locationButton[j].interactable = true;
    }

}
