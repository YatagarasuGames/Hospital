using Mirror;
using UnityEngine;

public class LocalUICreator : NetworkBehaviour, IInteract
{
    [SerializeField] protected GameObject _ui;

    [Command]
    public virtual void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }

    [Server]
    public virtual void Interact(uint interactCaller)
    {
        if (NetworkServer.spawned.TryGetValue(interactCaller, out NetworkIdentity interactPlayer))
        {
            TargetCreateTaskUI(interactPlayer.connectionToClient);
        }
    }

    [TargetRpc]
    protected virtual void TargetCreateTaskUI(NetworkConnectionToClient connection)
    {
        var serverPlayerUIDrawer = NetworkClient.localPlayer.GetComponent<PlayerUIDrawer>();
        serverPlayerUIDrawer.CreateUI(_ui, gameObject);

    }
}
