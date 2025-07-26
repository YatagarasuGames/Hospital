using UnityEngine;
using Mirror;

public class DoorLock : NetworkBehaviour, IInteract
{
    [SerializeField] private Door _door;
    [SyncVar] private bool _wasUsed = false;

    [Command]
    public void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }

    [Server]
    public void Interact(uint interactCaller)
    {
        if(_wasUsed) return;
        if(_door.Lock()) _wasUsed = true;
    }
}
