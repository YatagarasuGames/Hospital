using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCollectService : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _juice;

    private void Update()
    {
        if (isOwned)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isServer) Collect();
                else CmdCollect();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isServer) CreateJuice();
                else CmdCreateJuice();
            }
        }
    }

    [Server]
    public void Collect()
    {
        RaycastHit hit;
        Debug.DrawRay(_camera.position, _camera.forward);
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, 2f))
        {
            if(hit.collider.gameObject.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();

            }            
        }
    }

    [Command]
    public void CmdCollect()
    {
        Collect();
    }

    [Server]
    public void CreateJuice()
    {
        var juice = Instantiate(_juice);
        NetworkServer.Spawn(juice);
    }

    [Command]
    public void CmdCreateJuice()
    {
        CreateJuice();
    }
}
