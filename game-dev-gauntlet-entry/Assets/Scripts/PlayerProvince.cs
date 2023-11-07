using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProvince : MonoBehaviour
{
    
    public int provinceTotal;
    private int provinceUnlocked;

    public Button[] LocationButton;
    public Image[] LocationMarker;
    public GameObject[] ObjectButton;
    public GameObject[] MapImage;
    public Sprite UnlockLocation;
    public Sprite LockLocation;
    public Button BackDescButton;

    private void Awake()
    {
        provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        UpdateProvince();
    }

    private void UpdateProvince()
    {
        for (int i = 0; i < LocationMarker.Length; i++)
        {
            LocationMarker[i].sprite = LockLocation;
            LocationButton[i].interactable = false;
            ObjectButton[i].SetActive(false);
            MapImage[i].SetActive(false);
        }
        for (int i = 0; i < PlayerPrefs.GetInt("ProvinceUnlocked", 1); i++)
        {
            if (i < provinceTotal)
            {
                LocationMarker[i].sprite = UnlockLocation;
                LocationButton[i].interactable = true;
                ObjectButton[i].SetActive(true);
                MapImage[i].SetActive(true);
            }    
        }
    }

    public void DisableProvince()
    {
        BackDescButton.interactable = true;
        for (int j = 0; j < LocationMarker.Length; j++)
        {
            LocationButton[j].interactable = false;
        }
    }

    public void EnableProvince()
    {
        BackDescButton.interactable = false;
        StartCoroutine(DelayEnable());
    }

    private IEnumerator DelayEnable()
    {
        yield return new WaitForSeconds(1);
        for (int j = 0; j < PlayerPrefs.GetInt("ProvinceUnlocked", 1); j++)
        {
            if (j < provinceTotal)
            {
                LocationButton[j].interactable = true;
            }
        }
    }

}
