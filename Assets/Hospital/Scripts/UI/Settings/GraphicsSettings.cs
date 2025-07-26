using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : SettingsUnit
{
    [SerializeField] private TMP_Dropdown _resolution;
    [SerializeField] private Toggle _fullscreen;
    [SerializeField] private TMP_Dropdown _quality;
    private Resolution[] _resolutions;

    private int _currentResolutionIndex;

    public override void Start()
    {
        base.Start();
        InitResolutions();
    }

    private void InitResolutions()
    {
        _resolution.ClearOptions();
        List<string> options = new List<string>();
        _resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width
                  && _resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        _resolution.AddOptions(options);
        _resolution.RefreshShownValue();
        _currentResolutionIndex = currentResolutionIndex;
    }

    protected override void LoadSettings()
    {
        SetUiSettings();
    }

    protected override void SetUiSettings()
    {
        if (PlayerPrefs.HasKey("QualitySettings")) _quality.value = PlayerPrefs.GetInt("QualitySettings");
        else _quality.value = 3;

        if (PlayerPrefs.HasKey("ResolutionSettings")) _resolution.value = PlayerPrefs.GetInt("ResolutionSettings");
        else _resolution.value = _currentResolutionIndex;

        if (PlayerPrefs.HasKey("FullscreenPreference")) _fullscreen.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else _fullscreen.isOn = true;
    }

    public void SetResolution(int resolutionIndex)
    {
        bool fullscreen;
        if (PlayerPrefs.HasKey("FullscreenPreference"))
            fullscreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            fullscreen = true;

        Resolution _newResolution = _resolutions[resolutionIndex];
        Screen.SetResolution(_newResolution.width, _newResolution.height, fullscreen);
        PlayerPrefs.SetInt("ResolutionSettings", _resolution.value);
        Save();
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(isFullscreen));
        Save();
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualitySettings", _quality.value);
        Save();
    }
}
