using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float volumeMusic;
    public float volumeSfx;

    public AudioSource audioSourceMusic;
    public AudioClip mainMusic;
    public AudioClip[] provinceThemeMusic;
    public int[] provinceThemeInitialTime;
    public AudioClip kitchenMusic;
    public AudioClip restaurantMusic;

    public AudioSource startSfx;
    public AudioSource successSfx;
    public AudioSource failSfx;
    public AudioSource breakSfx;
    public AudioSource popSfx;

    private static Dictionary<string, GameObject> _instances = new Dictionary<string, GameObject>();
    public string ID;
    
    private void Awake()
    {
        // Will not destroy the script when on the next loaded scene
        if(_instances.ContainsKey(ID))
        {
            var existing = _instances[ID];
            if(existing != null)
            {
                if(ReferenceEquals(gameObject, existing))
                    return;

                Destroy(gameObject);
                return;
            }
        }

        _instances[ID] = gameObject;
        DontDestroyOnLoad(gameObject);

        // Reference the scripts from game objects
        audioSourceMusic = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Automatically update the volume for music
        volumeMusic = PlayerPrefs.GetFloat("GlobalVolumeMusic", 1);
        audioSourceMusic.volume = PlayerPrefs.GetFloat("GlobalVolumeMusic", 1);

        // Automatically update the volume for sfx
        volumeSfx = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1);
        startSfx.volume = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1) * 0.75f;
        successSfx.volume = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1);
        failSfx.volume = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1);
        breakSfx.volume = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1) * 0.5f;
        popSfx.volume = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1) * 0.5f;
    }

    public void PlayBackgroundMusic(AudioClip audioClip, bool shouldLoop)
    {
        // Set the audio source with the audio clip
        // Only for playing background audio
        audioSourceMusic.clip = audioClip;
        audioSourceMusic.loop = shouldLoop;
        audioSourceMusic.Play();
    }

    public void PlayThemeMusic(AudioClip audioClip, bool shouldLoop, int levelId)
    {
        // Set the audio source with the audio clip
        // Only for playing the theme of a selected province
        audioSourceMusic.clip = audioClip;
        audioSourceMusic.loop = shouldLoop;
        audioSourceMusic.Play();
        audioSourceMusic.time = provinceThemeInitialTime[levelId - 1];
    }

    public void StopMusic()
    {
        // Stop the music
        audioSourceMusic.loop = false;
        audioSourceMusic.Stop();
    }
}