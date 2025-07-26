using Mirror;
using UnityEngine;

public class PlayerInteractService : InputComponent
{
    [SerializeField] private Transform _camera;
    [SyncVar]private uint _playerId;

    public override void OnStartClient()
    {
        if (!isOwned) return;
        Init();
    }

    private void Update()
    {
        if(!isOwned || !_isActive) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isServer) Interact();
            else CmdInteract();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (isServer) LongInteract();
            else CmdLongInteract();
        }

        if(isServer && Input.GetKeyDown(KeyCode.K))
        {
            NetworkManager.singleton.ServerChangeScene("Game");
        }
    }

    private void Init() => _playerId = GetComponentInParent<NetworkIdentity>().netId;

    [Server]
    public void Interact()
    {
        if(_playerId == 0) _playerId = GetComponentInParent<NetworkIdentity>().netId;
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, 2f))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteract interactable))
            {
                if (hit.collider.gameObject.TryGetComponent(out IAffiliated affiliated))
                    if (!affiliated.HasAccess(_playerId)) return;
                interactable.Interact(_playerId);
            }
        }
    }

    [Command]
    public void CmdInteract()
    {
        if (_playerId == 0) _playerId = GetComponentInParent<NetworkIdentity>().netId;
        Interact();
    }

    [Server]
    public void LongInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, 2f))
        {
            if (hit.collider.gameObject.TryGetComponent(out ILongInteractable interactable))
            {
                if (hit.collider.gameObject.TryGetComponent(out IAffiliated affiliated))
                    if (!affiliated.HasAccess(_playerId)) return;
                interactable.InteractStep(_playerId);
            }
        }
    }

    [Command]
    public void CmdLongInteract()
    {
        LongInteract();
    }
}
