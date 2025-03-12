using Mirror;
using UnityEngine;

public class PlayerInputServices : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    private PlayerLongInteractService _playerLongInteractService;
    private PlayerCollectService _playerCollectService;
    private Inventory _inventory;
    [SerializeField] private GameObject _juice;

    public override void OnStartClient()
    {
        Init();
        Application.targetFrameRate = 60;
        for(int i = 0;  i < 1; i++) { NetworkServer.Spawn(Instantiate(_juice)); }
    }

    private void Init()
    {
        _inventory = new Inventory();
        _playerLongInteractService = new PlayerLongInteractService(_camera);
        _playerCollectService = new PlayerCollectService(_camera, _inventory);
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

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (isServer) HandleInventoryInput(1);
                else CmdHandleInventoryInput(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (isServer) HandleInventoryInput(2);
                else CmdHandleInventoryInput(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (isServer) HandleInventoryInput(3);
                else CmdHandleInventoryInput(3);
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

    [Server]
    private void HandleInventoryInput(int inventoryCell)
    {
        //Instantiate(_inventory.Get(inventoryCell - 1));
        NetworkServer.Spawn(Instantiate(_inventory.Get(inventoryCell - 1)));
    }

    [Command]
    private void CmdHandleInventoryInput(int inventoryCell)
    {
        HandleInventoryInput(inventoryCell);
    }
}
