using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public abstract class SettingsUnit : MonoBehaviour
{
    public static Action onSettingsChanged;
    public UnityEvent _onSettingsChanged;

    public virtual void Start()
    {
        SettingsLoader.OnSettingsLoadRequest += LoadSettings;
    }

    protected abstract void LoadSettings();
    protected abstract void SetUiSettings();

    public virtual void Save()
    {
        onSettingsChanged?.Invoke();
        _onSettingsChanged?.Invoke();
    }

    private void OnDisable()
    {
        SettingsLoader.OnSettingsLoadRequest -= LoadSettings;
    }
}
