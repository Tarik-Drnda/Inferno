using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    public AudioSource dropItemSound;
    public AudioSource pickUpItem;
    public AudioSource swingSound;
    public AudioSource walkingSound;
    public AudioSource jumpingSound;
    public AudioSource runningSound;

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
    
}
