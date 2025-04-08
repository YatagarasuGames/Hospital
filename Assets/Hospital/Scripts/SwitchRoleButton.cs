using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SwitchRoleButton : NetworkBehaviour, IInteract
{
    [SyncVar] private bool _wasClicked = false;
    [SerializeField] private Transform _newSpawnpointPosition;

    [Server]
    public void Interact()
    {
        
    }

    [Command]
    public void CmdInteract()
    {
        
    }
}
