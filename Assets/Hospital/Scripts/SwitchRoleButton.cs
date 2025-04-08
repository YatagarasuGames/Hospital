using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SwitchRoleButton : NetworkBehaviour, IInteract
{
    [SyncVar] public bool _wasClicked = false;
    [SerializeField] private string newSpawnpointPositionName;
    private Vector3 _newSpawnpointPosition;

    private void OnEnable()
    {
        _newSpawnpointPosition = GameObject.Find(newSpawnpointPositionName).transform.position;
    }

    [Server]
    public void Interact(uint interactCaller)
    {
        if(!_wasClicked && NetworkServer.spawned.TryGetValue(interactCaller, out NetworkIdentity player))
        {
            _wasClicked = true;
            player.GetComponent<NetworkTransformReliable>().ServerTeleport(_newSpawnpointPosition, Quaternion.identity);
        }
    }

    [Command]
    public void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }
}
