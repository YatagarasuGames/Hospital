using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : SettingsUnit
{
    [SerializeField] private Slider sensitivitySlider;
    protected override void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Sensitivity")) SetSensitivity(PlayerPrefs.GetFloat("Sensitivity"));
        else SetSensitivity(2f);
    }

    protected override void SetUiSettings() { }

    public void SetSensitivity(float sliderValue)
    {
        sensitivitySlider.value = sliderValue;
        float newSensitivity = sliderValue;
        PlayerPrefs.SetFloat("Sensitivity", newSensitivity);
        Save();
    }
}
