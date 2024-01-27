using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float volume;
    public AudioSource mainAudio;
    public AudioSource kitchenAudio;
    public AudioSource[] provinceThemeAudio;
    public AudioSource startSfx;
    public AudioSource successSfx;
    public AudioSource failSfx;
    public AudioSource breakSfx;
    public AudioSource popSfx;

    private static GameObject instanceAudioManager {set; get;}
    private void Awake()
    {
        if (instanceAudioManager != null) 
            Destroy(instanceAudioManager);

        instanceAudioManager = gameObject;
        DontDestroyOnLoad(instanceAudioManager);
    }

    private void Update()
    {
        volume = PlayerPrefs.GetFloat("GlobalVolume", 1);
    }

    public void PlayMainAudio()
    {
        mainAudio.Play();
    }
}