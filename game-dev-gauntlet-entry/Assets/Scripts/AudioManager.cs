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
        // Will not Destroy the Script When on the Next Scene
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

        // Referencing the Scripts from GameObjects
        audioSourceMusic = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Music Update
        volumeMusic = PlayerPrefs.GetFloat("GlobalVolumeMusic", 1);
        audioSourceMusic.volume = PlayerPrefs.GetFloat("GlobalVolumeMusic", 1);

        // SFX Update
        volumeSfx = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1);
        startSfx.volume = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1) * 0.75f;
        successSfx.volume = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1);
        failSfx.volume = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1);
        breakSfx.volume = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1) * 0.5f;
        popSfx.volume = PlayerPrefs.GetFloat("GlobalVolumeSfx", 1) * 0.5f;
    }

    public void PlayBackgroundMusic(AudioClip audioClip)
    {
        // Only for Playing Background Music
        audioSourceMusic.clip = audioClip;
        audioSourceMusic.Play();
    }

    public void PlayThemeMusic(AudioClip audioClip, int levelId)
    {
        // Only for Playing Province Theme Music
        audioSourceMusic.clip = audioClip;
        audioSourceMusic.Play();
        audioSourceMusic.time = provinceThemeInitialTime[levelId - 1];
    }

    public void StopMusic()
    {
        // Stops the Music
        audioSourceMusic.Stop();
    }
}