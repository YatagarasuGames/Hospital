using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerCollectService
{
    private Transform _camera;
    private Inventory _inventory;

    public PlayerCollectService(Transform camera, Inventory inventory)
    {
        _camera = camera;
        _inventory = inventory;
    }

    [Server]
    public void Collect()
    {
        RaycastHit hit;
        Debug.DrawRay(_camera.position, _camera.forward);
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, 2f))
        {
            if(hit.collider.gameObject.TryGetComponent(out CollectableItem collectable))
            {
                _inventory.Add((byte)collectable.Type);
                collectable.Collect();
                
                
            }            
        }
    }

    [Command]
    public void CmdCollect()
    {
        Collect();
    }
}
