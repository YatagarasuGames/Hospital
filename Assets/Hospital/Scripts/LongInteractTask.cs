using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LongInteractTask : NetworkBehaviour, ILongInteractable
{
    [SyncVar] protected bool _isCompleted = false;
    [SyncVar, SerializeField] protected float _completeDuration;
    [SyncVar(hook = nameof(HandleActivationProgressChanged))] protected float _completeProgress;

    protected InteractTip _interactTip;

    [SyncVar] protected NetworkIdentity _interacterPlayer;
    public NetworkIdentity InteracterPlayer => _interacterPlayer;
    protected IConcentrationProvider _concentrationProvider;


    [Server]
    public virtual void CompleteInteract()
    {
        _isCompleted = true;
    }

    [Server]
    public virtual void InteractStep(uint interactCaller)
    {
        if (_isCompleted) return;
        if (_interacterPlayer == null || _interacterPlayer.netId != interactCaller) UpdateInteracter(interactCaller);
        float multiplier = _concentrationProvider?.GetConcentration() ?? 1f;
        _completeProgress += Time.deltaTime * multiplier;
        if (_completeProgress >= _completeDuration) CompleteInteract();
    }

    [Server]
    private void UpdateInteracter(uint interactCaller)
    {
        _interacterPlayer = NetworkServer.spawned[interactCaller];
        _concentrationProvider = _interacterPlayer.GetComponent<IConcentrationProvider>();
    }

    protected virtual void HandleActivationProgressChanged(float old, float newProgress)
    {
        if (transform.childCount == 0) return;
        if (_interactTip == null) _interactTip = GetComponentInChildren<InteractTipUI>();
        (_interactTip as InteractTipUI).UpdateProgress(newProgress / _completeDuration);
    }
}
