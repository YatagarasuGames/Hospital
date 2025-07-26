using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class QuickTimeEventTask : LongInteractTask
{
    [SerializeField] protected bool _enableQuickTimeEvents = true;
    [SerializeField] private GameObject _quickTimeEventTaskObject;
    [SerializeField] protected float _quickTimeEventAppearChance;
    [SyncVar] protected bool _isQuickTimeEventCreated = false;

    [SyncVar] protected float _timeBetweenChecksForQuickTimeEventCreation = 2;
    [SyncVar] protected float _lastQuickTimeEventCreationCheckTime;

    [Server]
    public override void InteractStep(uint interactCaller)
    {
        if(_isCompleted || _isQuickTimeEventCreated) return;
        if (_interacterPlayer == null || _interacterPlayer.netId != interactCaller) UpdateInteracter(interactCaller); 
        float multiplier = _concentrationProvider?.GetConcentration() ?? 1f;
        _completeProgress += Time.deltaTime * multiplier;
        if(_enableQuickTimeEvents) TryCreateQuickTimeEvent(interactCaller);
        if(_completeProgress >= _completeDuration) CompleteInteract();
    }

    [Server]
    private void UpdateInteracter(uint interactCaller)
    {
        _interacterPlayer = NetworkServer.spawned[interactCaller];
        _concentrationProvider = _interacterPlayer.GetComponent<IConcentrationProvider>();
    }

    [Server]
    private void TryCreateQuickTimeEvent(uint interactCaller)
    {
        if (Time.time < _lastQuickTimeEventCreationCheckTime + _timeBetweenChecksForQuickTimeEventCreation) return;
        int percent = Random.Range(0, 100);
        if(percent > _quickTimeEventAppearChance)
        {
            _isQuickTimeEventCreated = true;
        }
        _lastQuickTimeEventCreationCheckTime = Time.time;

        CreateQuickTimeEvent(NetworkServer.spawned[interactCaller].connectionToClient);
    }

    [TargetRpc]
    private void CreateQuickTimeEvent(NetworkConnectionToClient connection)
    {
        var serverPlayerUIDrawer = NetworkClient.localPlayer.GetComponent<PlayerUIDrawer>();
        serverPlayerUIDrawer.CreateUI(_quickTimeEventTaskObject, gameObject);
    }

    [Command(requiresAuthority = false)]
    public void CmdHandleQuickTimeEventCompleted(bool result)
    {
        HandleQuickTimeEventCompleted(result);
    }

    [Server]
    private void HandleQuickTimeEventCompleted(bool result)
    {
        if (result) _completeProgress += 10;
        else { _completeProgress = Mathf.Clamp(_completeProgress -= 5, 0, _completeDuration); _concentrationProvider.ChangeConcentration(-1); }
        _isQuickTimeEventCreated = false;
    }
}
