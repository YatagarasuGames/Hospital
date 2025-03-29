using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MotionSensor : NetworkBehaviour
{
    [SyncVar] private bool _isEnabled = true;
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if(!_isEnabled) return;
        if (other.gameObject.CompareTag("Survivor"))
        {
            _isEnabled = false;
            other.gameObject.GetComponent<NetworkOutline>().isOutlined = true;
            RpcSetOutlinedMaterial(other.gameObject.GetComponent<NetworkIdentity>().netId);
        }
    }

    [ClientRpc]
    private void RpcSetOutlinedMaterial(uint netId)
    {
        if (NetworkServer.spawned.TryGetValue(netId, out var instance))
        {
            instance.gameObject.GetComponent<NetworkOutline>().isOutlined = true;
            print("Added");
        }
        
    }
}
