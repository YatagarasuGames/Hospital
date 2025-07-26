using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritTeleportModule : ActiveModule
{
    [SyncVar] private GameObject _markPlacer;
    protected override void OnBufferEnabled(GameObject oldPlayer, GameObject newPlayer)
    {
        _markPlacer = transform.parent.GetComponentInChildren<SpiritMarkPlacerModule>().gameObject;
    }

    [Server]
    protected override void Use()
    {
        base.Use();
        GameObject placedMark = _markPlacer.GetComponent<SpiritMarkPlacerModule>().PlacedSpiritMark;
        if (placedMark == null) return;

        _player.GetComponent<NetworkTransformReliable>().ServerTeleport(placedMark.transform.position, Quaternion.identity);
        
    }
}
