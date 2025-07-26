using UnityEngine;
using Mirror;

public class PlayerCollectService : InputComponent
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Inventory _inventory;

    private void Update()
    {
        if(!isOwned || !_isActive) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isServer) HandleInput();
            else CmdHandleInput();
        }
    }

    [Command]
    public void CmdHandleInput()
    {
        HandleInput();
    }

    [Server]
    public void HandleInput()
    {
        if (!_inventory.CanCollectItem()) return;
        RaycastHit hit;
        Debug.DrawRay(_camera.position, _camera.forward);
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, 2f))
        {
            if (hit.collider.gameObject.TryGetComponent(out CollectableItem collectable))
            {
                if (!CanCollectByAffiliation(collectable.GetComponent<NetworkIdentity>().netId)) return;
                _inventory.Add((byte)collectable.Type);
                collectable.Collect();
            }
        }
    }

    [Server]
    private bool CanCollectByAffiliation(uint itemNetId)
    {
        if(NetworkServer.spawned.TryGetValue(itemNetId, out NetworkIdentity item))
        {
            uint interacterId = 0;
            if (connectionToClient != null) interacterId = connectionToClient.identity.netId;
            else interacterId = NetworkClient.localPlayer.netId;

            return item.GetComponent<IAffiliated>().HasAccess(interacterId);
        }
        return false;
    }
}
