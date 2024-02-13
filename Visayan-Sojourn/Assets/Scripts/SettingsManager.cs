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
        // Reference the scripts from game objects
        _audioManager = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        _levelLoad = GameObject.FindGameObjectWithTag("mainScript").GetComponent<LevelLoad>();
        
        // Set initial values to the variables and set the game objects
        float volumeMusic = PlayerPrefs.GetFloat("GlobalVolumeMusic", 1);
        volumeMusicImage.sprite = (volumeMusic > 0) ? volumeMusicSprite[1] : volumeMusicSprite[0];
        volumeMusicSlider.value = volumeMusic;

        float volumeSfx = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1);
        volumeSfxImage.sprite = (volumeSfx > 0) ? volumeSfxSprite[1] : volumeSfxSprite[0];
        volumeSfxSlider.value = volumeSfx;
    }

    private void Update()
    {
        // Automatically update the music volume based on the value of the slider and update the music display
        PlayerPrefs.SetFloat("GlobalVolumeMusic", volumeMusicSlider.value);
        volumeMusicImage.sprite = (_audioManager.volumeMusic > 0) ? volumeMusicSprite[1] : volumeMusicSprite[0];

        // Automatically update the sfx volume based on the value of the slider and update the sfx display
        PlayerPrefs.SetFloat("GlobalVolumeSfx", volumeSfxSlider.value);
        volumeSfxImage.sprite = (_audioManager.volumeSfx > 0) ? volumeSfxSprite[1] : volumeSfxSprite[0];
    }

    public void OpenSettings()
    {
        // Open the settings interface
        _levelLoad.settingsPanel.SetActive(true);
        settingsController.SetTrigger("OpenSettings");
        mainSettingsPart.SetActive(true);
        creditsPart.SetActive(false);
    }

    public void CloseSettings()
    {        
        // Close the settings interface
        settingsController.SetTrigger("CloseSettings");
    }

    public void VolumeMusicSet()
    {
        // Set the music volume by muting or unmuting
        bool isVolumeGreaterZero = _audioManager.volumeMusic > 0;
        PlayerPrefs.SetFloat("GlobalVolumeMusic", (isVolumeGreaterZero) ? 0 : 0.5f);
        volumeMusicImage.sprite = (isVolumeGreaterZero) ? volumeMusicSprite[1] : volumeMusicSprite[0];
        volumeMusicSlider.value = (isVolumeGreaterZero) ? 0 : 0.5f;
    }

    public void VolumeSfxSet()
    {
        // Set the sfx volume by muting or unmuting
        bool isVolumeGreaterZero = _audioManager.volumeSfx > 0;
        PlayerPrefs.SetFloat("GlobalVolumeSfx", (isVolumeGreaterZero) ? 0 : 0.5f);
        volumeSfxImage.sprite = (isVolumeGreaterZero) ? volumeSfxSprite[1] : volumeSfxSprite[0];
        volumeSfxSlider.value = (isVolumeGreaterZero) ? 0 : 0.5f;
    }

    public void DisplayCredits()
    {
        // Display the credits on settings interface
        mainSettingsPart.SetActive(false);
        creditsPart.SetActive(true);
    }

    public void DoQuit()
    {
        // Quit the application if the quit button is pressed
        Application.Quit();
    }
}
