using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelManager : MonoBehaviour
{
    public string[] firstTimeKeyName = {"FirstTimeAntique", "FirstTimeAklan", "FirstTimeCapiz", "FirstTimeNegrosOcc", "FirstTimeGuimaras", "FirstTimeIloilo"};
    private VideoRender _videoRender;

    public void Awake()
    {
        _videoRender = GameObject.FindGameObjectWithTag("videoRender").GetComponent<VideoRender>();
        int provinceUnlocked = PlayerPrefs.GetInt("ProvinceUnlocked", 1);
        
        _videoRender.PlayTravel(provinceUnlocked);
        PlayerPrefs.SetInt(firstTimeKeyName[provinceUnlocked - 1], 0);
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainScene");
    }
}
