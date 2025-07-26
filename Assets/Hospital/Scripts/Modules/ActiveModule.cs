using Mirror;
using System;
using UnityEngine;

/// <summary>
/// <para>Base class for active modules</para>
/// <para>Override server Use() method to change functionality</para>
/// </summary>
public abstract class ActiveModule : Module, IDeadStateDependable
{
    [SerializeField] protected KeyCode _inputKey;
    [SerializeField] protected float _cooldown;
    [SyncVar] protected float _nextUseTime;
    [SyncVar] private bool _isActive = true;
    public static Action<ModuleData, float> OnAbilityUsed;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        OnModuleCreated?.Invoke(NetworkClient.localPlayer.netId, ModuleData, _inputKey.ToString());
    }

    public virtual void Update()
    {

        if (!isOwned || !_isActive) return;
        if (Input.GetKeyDown(_inputKey))
        {
            if (NetworkTime.time < _nextUseTime) return;
            OnAbilityUsed.Invoke(ModuleData, _cooldown);
            if (isServer) Use();
            else CmdUse();
        }
    }

    [Server]
    public void ChangeState(bool newState)
    {
        _isActive = newState;
    }

    [Command]
    public virtual void CmdUse()
    {
        Use();
    }

    [Server]
    protected virtual void Use()
    {

        _nextUseTime = (float)NetworkTime.time + _cooldown;
    }
}
