using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    public AudioSource currentBackgroundMusic;
    private List<AudioSource> audioQueue = new List<AudioSource>(4);

    private void Awake()
    {
        foreach(AudioClip audioClip in Resources.LoadAll<AudioClip>("Audio").ToArray())
        {
            audioClips.Add(audioClip.name, audioClip);
        }
    }

    public void PauseAll()
    {
        AudioListener.pause = true;
    }

    public void StopAudio(AudioSource audioSource)
    {
        audioQueue.Remove(audioSource);
        audioSource.Stop();
        Destroy(audioSource);
    }

    public void PlayBackgroundMusic(AudioClip audioClip, float fadeDuration, float targetVolume, float startTimeStamp)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = audioClip;
        newAudioSource.time = startTimeStamp;
        newAudioSource.loop = true;

        StartCoroutine(FadeOut(currentBackgroundMusic, fadeDuration));
        currentBackgroundMusic = newAudioSource;
        StartCoroutine(FadeIn(newAudioSource, fadeDuration, targetVolume));
    }

    public void PlayAudio(AudioClip audioClip, float startTimestamp)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = audioClip;
        newAudioSource.time = startTimestamp;

        if (audioQueue.Count == audioQueue.Capacity)
        {
            AudioSource firstInQueue = audioQueue[0];
            audioQueue.RemoveAt(0);
            Destroy(firstInQueue);
        }
        audioQueue.Add(newAudioSource);
    }

    public void FadeInAudio(AudioClip audioClip, float fadeDuration, float targetVolume)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = audioClip;

        if (audioQueue.Count == audioQueue.Capacity)
        {
            AudioSource firstInQueue = audioQueue[0];
            audioQueue.RemoveAt(0);
            Destroy(firstInQueue);
        }
        audioQueue.Add(newAudioSource);

        StartCoroutine(FadeIn(newAudioSource, fadeDuration, targetVolume));
    }

    public void FadeInAudio(AudioClip audioClip, float fadeDuration, float targetVolume, float startTimestamp)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = audioClip;
        newAudioSource.time = startTimestamp;

        StartCoroutine(FadeIn(newAudioSource, fadeDuration, targetVolume));
    }

    public void FadeOutAudio(AudioSource audioSource, float fadeDuration)
    {
        StartCoroutine(FadeOut(audioSource, fadeDuration));
    }

    IEnumerator FadeIn(AudioSource audioSource, float fadeDuration, float targetVolume)
    {
        float currentTime = 0;
        float startVolume = 0;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, currentTime / fadeDuration);
            yield return null;
        }
    }

    IEnumerator FadeOut(AudioSource audioSource, float fadeDuration)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / fadeDuration);
            yield return null;
        }

        audioSource.Stop(); // kailangan pa ba to?
        Destroy(audioSource);
        Debug.Log("deleted");
    }
}
