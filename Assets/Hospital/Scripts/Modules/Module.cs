using UnityEngine;
using Mirror;
using System;

public abstract class Module : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnBufferEnabled))] protected GameObject _player;

    public static Action<uint, ModuleData, string> OnModuleCreated;
    public static Action<uint, ModuleData> OnModuleDestroyed;

    [field: SerializeField] public ModuleData ModuleData { get; private set; }



    /// <summary>
    /// <para>In this function you get player in newBuffer GameObject.</para>
    /// <para>Use player GameObject to get dependencies</para>
    /// <para>Call ApplyBonus() function if isServer in case of passive module here</para>
    /// </summary>
    protected abstract void OnBufferEnabled(GameObject oldPlayer, GameObject newPlayer);

    [Server]
    public void SetModulesObjectsBuffer(GameObject player) => _player = player;

    public override void OnStopClient()
    {
        base.OnStopClient();
        OnModuleDestroyed?.Invoke(NetworkClient.localPlayer.netId, ModuleData);
    }
}
