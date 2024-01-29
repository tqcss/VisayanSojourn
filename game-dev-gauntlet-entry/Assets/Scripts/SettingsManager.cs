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
    public Image volumeMusicImage;
    public Sprite[] volumeMusicSprite;
    public Slider volumeMusicSlider;
    public Image volumeSfxImage;
    public Sprite[] volumeSfxSprite;
    public Slider volumeSfxSlider;

    private AudioManager _audioManager;
    private LevelLoad _levelLoad;

    private void Awake()
    {
        // Referencing the Scripts from GameObjects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        
        float volumeMusic = PlayerPrefs.GetFloat("GlobalVolumeMusic", 1);
        volumeMusicImage.sprite = (volumeMusic > 0) ? volumeMusicSprite[1] : volumeMusicSprite[0];
        volumeMusicSlider.value = volumeMusic;

        float volumeSfx = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1);
        volumeSfxImage.sprite = (volumeSfx > 0) ? volumeSfxSprite[1] : volumeSfxSprite[0];
        volumeSfxSlider.value = volumeSfx;
    }

    private void Update()
    {
        // Volume Music Settings
        PlayerPrefs.SetFloat("GlobalVolumeMusic", volumeMusicSlider.value);
        volumeMusicImage.sprite = (_audioManager.volumeMusic > 0) ? volumeMusicSprite[1] : volumeMusicSprite[0];

        // Volume Sfx Settings
        PlayerPrefs.SetFloat("GlobalVolumeSfx", volumeSfxSlider.value);
        volumeSfxImage.sprite = (_audioManager.volumeSfx > 0) ? volumeSfxSprite[1] : volumeSfxSprite[0];
    }

    public void OpenSettings()
    {
        // Open the Settings UI
        _levelLoad.settingsPanel.SetActive(true);
        settingsController.SetTrigger("OpenSettings");
        mainSettingsPart.SetActive(true);
        creditsPart.SetActive(false);
    }

    public void CloseSettings()
    {        
        // Close the Settings UI
        settingsController.SetTrigger("CloseSettings");
    }

    public void VolumeMusicSet()
    {
        // Set Mute or Unmute
        bool isVolumeGreaterZero = _audioManager.volumeMusic > 0;
        PlayerPrefs.SetFloat("GlobalVolumeMusic", (isVolumeGreaterZero) ? 0 : 0.5f);
        volumeMusicImage.sprite = (isVolumeGreaterZero) ? volumeMusicSprite[1] : volumeMusicSprite[0];
        volumeMusicSlider.value = (isVolumeGreaterZero) ? 0 : 0.5f;
    }

    public void VolumeSfxSet()
    {
        // Set Mute or Unmute
        bool isVolumeGreaterZero = _audioManager.volumeSfx > 0;
        PlayerPrefs.SetFloat("GlobalVolumeSfx", (isVolumeGreaterZero) ? 0 : 0.5f);
        volumeSfxImage.sprite = (isVolumeGreaterZero) ? volumeSfxSprite[1] : volumeSfxSprite[0];
        volumeSfxSlider.value = (isVolumeGreaterZero) ? 0 : 0.5f;
    }

    public void DisplayCredits()
    {
        // Display Credits on Settings
        mainSettingsPart.SetActive(false);
        creditsPart.SetActive(true);
    }

    public void DoQuit()
    {
        Application.Quit();
    }
}
