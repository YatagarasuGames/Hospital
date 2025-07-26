using Mirror;
using UnityEngine;
using System;
using System.Collections.Generic;

public class ClassesController : NetworkBehaviour
{
    public static ClassesController Instance;

    [SerializeField] private List<NetworkIdentity> _classPickerGameobjects;
    private readonly SyncDictionary<ClassType, NetworkIdentity> _classPickers = new SyncDictionary<ClassType, NetworkIdentity>();

    private void Awake()
    {
        Instance ??= this;

    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        foreach (var identity in _classPickerGameobjects)
        {
            _classPickers.Add(identity.GetComponentInChildren<BaseClassSwitcher>().ClassType, identity);
        }

    }

    [Server]
    public void HandleClassSwitched(ClassType oldClassType, ClassType newClassType, NetworkIdentity callerPlayer)
    {
        if (oldClassType != ClassType.@default) _classPickers[oldClassType].GetComponentInChildren<BaseClassSwitcher>().RemoveClass();
        if (newClassType != ClassType.@default) _classPickers[newClassType].GetComponentInChildren<BaseClassSwitcher>().AppendClass(callerPlayer.netId);
    }

    [Server]
    public void HandlePlayerLeft(NetworkIdentity player)
    {
        foreach (var identity in _classPickers.Values)
        {
            BaseClassSwitcher classSwitcher = identity.GetComponentInChildren<BaseClassSwitcher>();
            if (classSwitcher.ClassOwner == player.netId)
            {
                classSwitcher.RemoveClass();
                return;
            }
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        _classPickers.Clear();
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
