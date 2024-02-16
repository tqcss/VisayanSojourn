using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelManager : MonoBehaviour
{
    public string[] primalTravelKeyNames = {"PrimalTravelAntique", "PrimalTravelAklan", "PrimalTravelCapiz", "PrimalTravelNegrosOcc", "PrimalTravelGuimaras", "PrimalTravelIloilo"};
    private VideoRender _videoRender;

    public void Awake()
    {
        _videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        if (PlayerPrefs.GetInt(primalTravelKeyNames[provinceUnlocked - 1], 1) == 1)
        {
            StartCoroutine(_videoRender.PlayTravel(provinceUnlocked));
            PlayerPrefs.SetInt(primalTravelKeyNames[provinceUnlocked - 1], 0);
        }
        else
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainScene");
    }
}
