using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolution;
    [SerializeField] private Toggle fullscreen;
    [SerializeField] private TMP_Dropdown quality;
    [SerializeField] private Slider sensitivitySlider;
    //[SerializeField] private Slider brightnessSlider;
    //[SerializeField] private LeanLocalization leanLocalization;
    Resolution[] resolutions;

    [Header("Звуки")]
    [Space(10)]
    [SerializeField] private Slider ambientSlider;
    [SerializeField] private Slider effectsSlider;
    private float disabledVolume = -80f;
    [SerializeField] private AudioMixer soundMixer;

    //[SerializeField] private BrightnessVolume brightness;

    public static Action onSettingsChanged;
    public UnityEvent _onSettingsChanged;

    private void OnEnable()
    {
        //brightness = FindObjectOfType<BrightnessVolume>();
        //if (leanLocalization) return;
        //leanLocalization = FindObjectOfType<LeanLocalization>();

    }
    private void Start()
    {
        resolution.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width
                  && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolution.AddOptions(options);
        resolution.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    public void LoadSettings(int currentResolutionIndex)
    {

        if (PlayerPrefs.HasKey("QualitySettings"))
            quality.value = PlayerPrefs.GetInt("QualitySettings");
        else
            quality.value = 3;

        if (PlayerPrefs.HasKey("ResolutionSettings"))
            resolution.value = PlayerPrefs.GetInt("ResolutionSettings");
        else
            resolution.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
        {
            fullscreen.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        }
        else
            fullscreen.isOn = true;

        //if (PlayerPrefs.HasKey("AmbientSound"))
        //    SetAmbientVolume(((float)PlayerPrefs.GetInt("AmbientSound")) / 100);
        //else
        //    SetAmbientVolume(0.5f);

        //if (PlayerPrefs.HasKey("EffectsSound"))
        //    SetEffectsVolume(((float)PlayerPrefs.GetInt("EffectsSound")) / 100);
        //else
        //    SetEffectsVolume(0.5f);

        if (PlayerPrefs.HasKey("Sensitivity"))
            SetSensitivity(PlayerPrefs.GetFloat("Sensitivity"));
        else SetSensitivity(2f);

        //if (PlayerPrefs.HasKey("Brightness"))
        //    SetBrightness((float)PlayerPrefs.GetInt("Brightness") / 10);
        //else SetBrightness(0.5f);

        //if (PlayerPrefs.HasKey("Language")) leanLocalization.SetCurrentLanguage(PlayerPrefs.GetString("Language"));

        onSettingsChanged?.Invoke();
        _onSettingsChanged?.Invoke();
    }
    public void Save()
    {
        onSettingsChanged?.Invoke();
        _onSettingsChanged?.Invoke();
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(isFullscreen));
        Save();
    }
    public void SetSensitivity(float sliderValue)
    {
        sensitivitySlider.value = sliderValue;
        float newSensitivity = sliderValue;
        PlayerPrefs.SetFloat("Sensitivity", newSensitivity);
        Save();
    }

    public void SetResolution(int resolutionIndex)
    {
        bool fullscreen;
        if (PlayerPrefs.HasKey("FullscreenPreference"))
            fullscreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            fullscreen = true;

        Resolution _resolution = resolutions[resolutionIndex];
        Screen.SetResolution(_resolution.width, _resolution.height, fullscreen);
        PlayerPrefs.SetInt("ResolutionSettings", resolution.value);
        Save();
    }

    //public void SetLanguage(string language)
    //{
    //    leanLocalization.CurrentLanguage = language;
    //    PlayerPrefs.SetString("Language", language);
    //    Save();
    //}
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualitySettings", quality.value);
        Save();
    }
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

    //public void SetBrightness(float brightnessSliderValue)
    //{
    //    float newBrightness = Mathf.Lerp(-1, 1, brightnessSliderValue);
    //    brightnessSlider.value = brightnessSliderValue;
    //    brightness.gameObject.GetComponent<Volume>().profile.TryGet(out ColorAdjustments colorAdjustments);
    //    colorAdjustments.postExposure.value = newBrightness;
    //    PlayerPrefs.SetInt("Brightness", Convert.ToInt32(brightnessSliderValue * 10));
    //    Save();
    //}
}
