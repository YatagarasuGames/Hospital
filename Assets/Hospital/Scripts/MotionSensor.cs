using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MotionSensor : NetworkBehaviour
{
    [SyncVar] private bool _isEnabled = true;
    [SerializeField] private Material _outlinedMaterial;
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if(!_isEnabled) return;
        if (other.gameObject.CompareTag("Survivor"))
        {
            _isEnabled = false;
            other.gameObject.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
            print(other.gameObject.GetComponent<Outline>());
            RpcSetOutlinedMaterial(other.gameObject.GetComponent<NetworkIdentity>().netId);
        }
    }

    [ClientRpc]
    private void RpcSetOutlinedMaterial(uint netId)
    {
        if (NetworkServer.spawned.TryGetValue(netId, out var instance))
        {
            instance.gameObject.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
            print("Added");
        }
        
    }
}
