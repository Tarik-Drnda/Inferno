using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    public AudioSource dropItemSound;
    public AudioSource pickUpItem;
    public AudioSource walkingSound;
    public AudioSource jumpingSound;
    public AudioSource runningSound;
    public AudioSource menuSound;
    public AudioSource swingSound;
    public AudioSource enemySound;

    public AudioSource startingZoneBGMusic;
   
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }
    
    public void StopAllSoundsAndPlayDesired(AudioSource desiredAudioSource)
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }

        desiredAudioSource.Play();
    }

    
}
