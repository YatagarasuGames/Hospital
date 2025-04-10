using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MotionSensor : NetworkBehaviour
{
    [SyncVar] private bool _isEnabled = true;
    [SerializeField] private float outlineTime;
    [SyncVar] private uint outlinedPlayerId;

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if(!_isEnabled) return;
        if (other.gameObject.CompareTag("Survivor"))
        {
            _isEnabled = false;
            other.gameObject.GetComponent<NetworkOutline>().SetOutlineFormat(true);
            outlinedPlayerId = other.gameObject.GetComponent<NetworkIdentity>().netId;
            //RpcSetOutlinedMaterial(outlinedPlayerId, true);

            StartCoroutine(StopOutline());
        }
    }

    private IEnumerator StopOutline()
    {
        yield return new WaitForSeconds(outlineTime);
        if (NetworkServer.spawned.TryGetValue(outlinedPlayerId, out var instance))
        {
            instance.gameObject.GetComponent<NetworkOutline>().SetOutlineFormat(false);
            //RpcSetOutlinedMaterial(outlinedPlayerId, false);
        }
    }

    /*[ClientRpc]
    private void RpcSetOutlinedMaterial(uint netId, bool isOutlined)
    {
        if (NetworkServer.spawned.TryGetValue(netId, out var instance))
        {
            instance.gameObject.GetComponent<NetworkOutline>().SetOutlineFormat(isOutlined);
        }

    }*/
}
