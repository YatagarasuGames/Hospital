using Mirror;
using UnityEngine;

public class ReadyUpButton : NetworkBehaviour, IInteract
{
    [SerializeField] private ReadyUpPlayerItem _readyUpItem;

    [SerializeField] private Material _readyMaterial;
    [SerializeField] private Material _notReadyMaterial;
    [SyncVar(hook = nameof(ChangeReadyState))] private bool _isReady = false;
    [SerializeField] private ReadyUpButton _hunterButton;
    public bool IsReady => _isReady;

    [Command]
    public void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }

    [Server]
    public void Interact(uint interactCaller)
    {
        if (_readyUpItem.UserNetId != interactCaller) return;
        if (NetworkServer.spawned.TryGetValue(interactCaller, out var player))
        {
            if (player.CompareTag("Untagged")) return;
        }
        else return;
        _isReady = !_isReady;
        _hunterButton._isReady = _isReady;
    }

    private void ChangeReadyState(bool oldState, bool newState)
    {
        if(newState) gameObject.GetComponent<MeshRenderer>().material = _readyMaterial;
        else gameObject.GetComponent<MeshRenderer>().material = _notReadyMaterial;
    }

    [Server]
    public void ResetReadyState()
    {
        _isReady = false;
        if(_hunterButton) _hunterButton._isReady = false;
    }
}
