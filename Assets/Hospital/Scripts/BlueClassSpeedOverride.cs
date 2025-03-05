using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueClassSpeedOverride : NetworkBehaviour
{
    private float _speedBoost = 10f;
    private void OnEnable()
    {
        GetComponent<PlayerMovement>().speedOverrides.Add(_speedBoost);
    }

    private void OnDisable()
    {
        GetComponent<PlayerMovement>().speedOverrides.Remove(_speedBoost);
    }
}
