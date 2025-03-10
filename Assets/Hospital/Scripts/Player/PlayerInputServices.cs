using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputServices : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    private PlayerLongInteractService _playerLongInteractService;
    
    public override void OnStartClient()
    {
        Init();
        Application.targetFrameRate = 60;
    }

    private void Init()
    {
        _playerLongInteractService = new PlayerLongInteractService(_camera);
    }

    private void Update()
    {
        if (isOwned)
        {
            if(Input.GetKey(KeyCode.E))
            {
                if (isServer) LongInteract();
                else CmdLongInteract();
            }
        }
    }

    [Server]
    private void LongInteract()
    {
        _playerLongInteractService.Interact();
    }
    [Command]
    private void CmdLongInteract()
    {
        _playerLongInteractService.CmdInetract();
    }
}
