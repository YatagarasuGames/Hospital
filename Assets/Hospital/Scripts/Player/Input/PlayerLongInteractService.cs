using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLongInteractService
{
    [SerializeField] private Transform _camera;

    public PlayerLongInteractService(Transform camera)
    {
        _camera = camera;
    }



    [Server]
    public void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, 2f))
        {
            //Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject.TryGetComponent(out ILongInteractable interactable))
            {
                interactable.InteractStep();
            }
        }
    }

    [Command]
    public void CmdInetract()
    {
        Interact();
    }
}
