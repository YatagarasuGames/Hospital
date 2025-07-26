using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsLoader : MonoBehaviour
{
    public static Action OnSettingsLoadRequest;

    private void Start()
    {
        OnSettingsLoadRequest?.Invoke();
    }
}
