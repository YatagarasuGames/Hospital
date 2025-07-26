using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// Base class for player input components.
/// </summary>
public abstract class InputComponent : NetworkBehaviour, IDeadStateDependable
{
    [SerializeField, SyncVar] protected bool _isActive = true;
    [SerializeField] private bool _mustBeChangedOnUiCreated = true;

    [Server]
    public void SetActiveState(bool isActive)
    {
        if (_mustBeChangedOnUiCreated == false) return;
        _isActive = isActive;
        if (_isActive) OnModuleEnabled();
        else OnModuleDisabled();
    }
    
    [Server]
    void IDeadStateDependable.ChangeState(bool newState)
    {
        _isActive = newState;
    }

    protected virtual void OnModuleEnabled() { }
    protected virtual void OnModuleDisabled() { }

}
