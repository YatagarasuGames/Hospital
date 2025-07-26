using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ElectricPanel : QuickTimeEventTask
{
    [SerializeField] private List<CeilLamp> _lamps;
    [SerializeField] private LightmapSwitcher _lightmapSwithcer;

    public override void OnStartServer()
    {
        NetworkServer.OnDisconnectedEvent += HandlePlayerLeft;
    }

    [Server]
    private void HandlePlayerLeft(NetworkConnectionToClient connection)
    {

        //if (connection == _interacterPlayer.connectionToClient || _isQuickTimeEventCreated) _isQuickTimeEventCreated = false;
    }

    [Server]
    public override void CompleteInteract()
    {
        base.CompleteInteract();
        foreach(CeilLamp lamp in _lamps)
        {
            lamp.ChangeLightState(true);
        }
        _lightmapSwithcer.SetBrightLightmap();
        RpcSetBrightLightMap();
    }

    [ClientRpc]
    private void RpcSetBrightLightMap()
    {
        _lightmapSwithcer.SetBrightLightmap();
    }
}
