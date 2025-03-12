using Mirror;
using UnityEngine;

public class PlayerInputServices : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    private PlayerLongInteractService _playerLongInteractService;
    private PlayerCollectService _playerCollectService;
    [SerializeField] private GameObject _juice;

    public override void OnStartClient()
    {
        Init();
        Application.targetFrameRate = 60;
        for(int i = 0;  i < 10; i++) { NetworkServer.Spawn(Instantiate(_juice)); }
    }

    private void Init()
    {
        _playerLongInteractService = new PlayerLongInteractService(_camera);
        _playerCollectService = new PlayerCollectService(_camera);
    }

    private void Update()
    {
        if (isOwned)
        {
            if(Input.GetKey(KeyCode.E))
            {
                if (isServer)
                {
                    LongInteract();

                }
                else CmdLongInteract();
            }

            if(Input.GetKeyDown(KeyCode.E))
            {
                if (isServer) Collect();
                else CmdCollect();
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

    [Server]
    private void Collect()
    {
        _playerCollectService.Collect();

    }
    [Command]
    private void CmdCollect()
    {
        _playerCollectService.CmdCollect();
    }
}
