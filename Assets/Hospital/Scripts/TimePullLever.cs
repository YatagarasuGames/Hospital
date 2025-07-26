using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePullLever : NetworkBehaviour, IInteract
{
    [SerializeField] private TimePullLeverController _controller;

    [SyncVar] private bool _isCompleted = false;
    [SyncVar] private bool _isPulled = false;
    [SerializeField] private float _deactivationTime;
    private float _deactivationTimer;
    private Animator Animator;

    public bool IsPulled => _isPulled;
    private void Update()
    {
        if (_isCompleted) return;
        if (_isPulled) _deactivationTimer += Time.deltaTime;
        if(_deactivationTimer >= _deactivationTime) DeactivateLever();
    }

    [Command]
    public void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }

    [Server]
    public void Interact(uint interactCaller)
    {
        if (_isCompleted) return;
        if (_isPulled) return;
        ActivateLever();
    }

    [Server]
    private void ActivateLever()
    {
        _isPulled = true;
        _controller.HandleLeverPulled();
        print("Lever Activated");
    }
    [Server]
    private void DeactivateLever()
    {
        _isPulled = false;
        _deactivationTimer = 0;
        print("Lever deactivated");
    }

    public void SetAsCompleted() => _isCompleted = true;
}
