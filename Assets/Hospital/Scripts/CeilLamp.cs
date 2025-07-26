using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilLamp : NetworkBehaviour
{
    [SerializeField] private Light _light;

    [Server]
    public void ChangeLightState(bool newState)
    {
        _light.enabled = newState;
        RpcChangeLightState(newState);
    }

    [ClientRpc]
    private void RpcChangeLightState(bool newState)
    {
        _light.enabled = newState;
    }
}
