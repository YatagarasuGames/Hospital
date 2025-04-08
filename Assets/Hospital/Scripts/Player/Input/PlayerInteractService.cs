using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractService
{
    [SerializeField] private Transform _camera;
    private uint _playerId;

    public PlayerInteractService(Transform camera, uint playerID)
    {
        _camera = camera;
        _playerId = playerID;
    }

    [Server]
    public void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, 2f))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteract interactable))
            {
                interactable.Interact(_playerId);
            }
        }
    }

    [Command]
    public void CmdInteract()
    {
        Interact();
    }
}
