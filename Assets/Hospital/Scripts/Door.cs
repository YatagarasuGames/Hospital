using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : NetworkBehaviour, IInteract
{
    [SerializeField] [SyncVar] private bool _isLocked = false;
    [SyncVar] private bool _isOpened = false;

    [SyncVar] private bool _isLockClosed = false;
    [SyncVar] private int _attemptsToUnlockDoor = 0;
    private int _attemptsToUnlockDoorNeeded = 6;

    [field: SerializeField] public DoorKeyType Type { get; private set; }


    [Command]
    public void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }

    [Server]
    public void Interact(uint interactCaller)
    {
        if(_isLocked) return;
        if (_isLockClosed)
        {
            _attemptsToUnlockDoor++;
            if (_attemptsToUnlockDoor == _attemptsToUnlockDoorNeeded) BreakLock();
            return;
        }
        if (_isOpened) { transform.rotation = Quaternion.Euler(0, 0f, 0); _isOpened = false; }
        else { transform.rotation = Quaternion.Euler(0, 90f, 0); _isOpened = true; }
    }

    [Server]
    public void Unlock()
    {
        _isLocked = false;
    }

    [Server]
    public void BreakLock()
    {
        _isLockClosed = false;
    }

    [Server]
    public bool Lock()
    {
        if(_isLockClosed) return false;
        if (_isOpened == false)
        {
            _isLockClosed = true;
            return true;
        }
        return false;
    }


}
