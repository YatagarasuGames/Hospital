using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AthleteSpeedOverride : NetworkBehaviour
{
    private float _speedBoost = 10f;
    private void OnEnable()
    {
        GetComponentInParent<PlayerMovement>().speedOverrides.Add(_speedBoost);
    }

    private void OnDisable()
    {
        GetComponentInParent<PlayerMovement>().speedOverrides.Remove(_speedBoost);
    }
}
