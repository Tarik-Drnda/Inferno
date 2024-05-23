using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; set; }

    public Button backBTN;
    
    public Slider masterSlider;
    public GameObject masterValue; 
    
    public Slider musicSlider;
    public GameObject musicValue; 
    
    public Slider effectsSlider;
    public GameObject effectsValue;
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

    private void Start()
    {
        
        masterSlider.onValueChanged.AddListener(UpdateMasterVolume);
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
        effectsSlider.onValueChanged.AddListener(UpdateEffectsVolume);
        
        
        backBTN.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveVolumeSetings(musicSlider.value, effectsSlider.value, masterSlider.value);
            print("Saved to Player Pref");
        });

      //  if (SaveManager.Instance.isLoading == true)
        //{
          //  StartCoroutine(LoadAndApplySettings());
       //}


    }
    private void UpdateMasterVolume(float volume)
    {
        SoundManager.Instance.startingZoneBGMusic.volume = volume;
        SoundManager.Instance.walkingSound.volume = volume;
        SoundManager.Instance.runningSound.volume = volume;
        SoundManager.Instance.dropItemSound.volume = volume;
        SoundManager.Instance.pickUpItem.volume = volume;
        SoundManager.Instance.jumpingSound.volume = volume;
    }
    private void UpdateMusicVolume(float volume)
    {
       
        SoundManager.Instance.startingZoneBGMusic.volume = volume;
    }
    
    private void UpdateEffectsVolume(float volume)
    {
        SoundManager.Instance.walkingSound.volume = volume;
        SoundManager.Instance.runningSound.volume = volume;
        SoundManager.Instance.dropItemSound.volume = volume;
        SoundManager.Instance.pickUpItem.volume = volume;
        SoundManager.Instance.jumpingSound.volume = volume;
    }
    
    private IEnumerator LoadAndApplySettings()
    {
        LoadAndSetVolume();
        yield return new WaitForSeconds(0.1f);
    }

    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();
        masterSlider.value = volumeSettings.master;
        musicSlider.value = volumeSettings.music;
        effectsSlider.value = volumeSettings.effects;
        
        print("Volume settings are loaded");
    }

    private void Update()
    {
        masterValue.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicValue.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        effectsValue.GetComponent<TextMeshProUGUI>().text = "" + (effectsSlider.value) + "";
    }
}
