using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Animator settingsController;
    public Button settingsButton;
    public GameObject mainSettingsPart;
    public GameObject creditsPart;
    public Button volumeButton;
    public Image volumeImage;
    public Sprite[] volumeSprite;
    public Slider volumeSlider;

    private AudioManager audioManager;
    private LevelLoad levelLoad;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        
        float volume = PlayerPrefs.GetFloat("GlobalVolume", 1);
        volumeImage.sprite = (volume > 0) ? volumeSprite[1] : volumeSprite[0];
        volumeSlider.value = volume;
    }

    private void Update()
    {
        // Volume Settings
        PlayerPrefs.SetFloat("GlobalVolume", volumeSlider.value);
        volumeImage.sprite = (audioManager.volume > 0) ? volumeSprite[1] : volumeSprite[0];
    }

    public void OpenSettings()
    {
        levelLoad.settingsPanel.SetActive(true);
        settingsController.SetTrigger("OpenSettings");
        mainSettingsPart.SetActive(true);
        creditsPart.SetActive(false);
    }

    public void CloseSettings()
    {        
        settingsController.SetTrigger("CloseSettings");
    }

    public void VolumeSetSound()
    {
        bool isVolumeGreaterZero = audioManager.volume > 0;
        PlayerPrefs.SetFloat("GlobalVolume", (isVolumeGreaterZero) ? 0 : 0.5f);
        volumeImage.sprite = (isVolumeGreaterZero) ? volumeSprite[1] : volumeSprite[0];
        volumeSlider.value = (isVolumeGreaterZero) ? 0 : 0.5f;
    }

    public void DisplayCredits()
    {
        mainSettingsPart.SetActive(false);
        creditsPart.SetActive(true);
    }

    public void DoQuit()
    {
        Application.Quit();
    }
}
