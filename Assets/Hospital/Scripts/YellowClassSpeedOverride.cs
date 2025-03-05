using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowClassSpeedOverride : NetworkBehaviour
{
    private float _speedBoost = 1f;
    private void OnEnable()
    {
        GetComponent<PlayerMovement>().speedOverrides.Add(_speedBoost);
    }

    private void OnDisable()
    {
        GetComponent<PlayerMovement>().speedOverrides.Remove(_speedBoost);
    }
}
