using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class AudioSettings : SettingsUnit
{
    [SerializeField] private Slider ambientSlider;
    [SerializeField] private Slider effectsSlider;
    private float disabledVolume = -80f;
    [SerializeField] private AudioMixer soundMixer;
    protected override void LoadSettings()
    {
        if (PlayerPrefs.HasKey("AmbientSound")) SetAmbientVolume(((float)PlayerPrefs.GetInt("AmbientSound")) / 100);
        else SetAmbientVolume(0.5f);

        if (PlayerPrefs.HasKey("EffectsSound")) SetEffectsVolume(((float)PlayerPrefs.GetInt("EffectsSound")) / 100);
        else SetEffectsVolume(0.5f);
    }

    protected override void SetUiSettings() { }

    public void SetAmbientVolume(float sliderVolume)
    {
        float newVolume;
        if (sliderVolume == 0) newVolume = disabledVolume;
        else newVolume = Mathf.Lerp(-10f, 30f, sliderVolume);
        soundMixer.SetFloat("ambientVolume", newVolume);
        ambientSlider.value = sliderVolume;
        PlayerPrefs.SetInt("AmbientSound", Convert.ToInt32(sliderVolume * 100));
    }
    public void SetEffectsVolume(float sliderVolume)
    {
        float newVolume;
        if (sliderVolume == 0) newVolume = disabledVolume;
        else newVolume = Mathf.Lerp(-10f, 30f, sliderVolume);
        soundMixer.SetFloat("effectsVolume", newVolume);
        effectsSlider.value = sliderVolume;
        PlayerPrefs.SetInt("EffectsSound", Convert.ToInt32(sliderVolume * 100));
    }
}
