using Mirror;
using UnityEngine;

public class PlayerInputServices : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    private PlayerLongInteractService _playerLongInteractService;
    private PlayerCollectService _playerCollectService;
    private Inventory _inventory;
    [SerializeField] private GameObject _juice;
    [SerializeField] private GameObject _trap;
    [SerializeField] private GameObject classSwithcer;

    public override void OnStartClient()
    {
        Init();
        Application.targetFrameRate = 60;
        if(isServer && isOwned) for(int i = 0;  i < 4; i++) { var tempJuice = Instantiate(_juice);  NetworkServer.Spawn(tempJuice); }
        if (isServer && isOwned) { var tempTrap = Instantiate(classSwithcer); NetworkServer.Spawn(tempTrap); }
        if (isServer && isOwned) { var tempTrap = Instantiate(_trap); NetworkServer.Spawn(tempTrap); }
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
                if (isServer) LongInteract();
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
        Collect();
    }

    [Server]
    private void HandleInventoryInput(int inventoryCell)
    {
        GameObject tempGO = _inventory.Get(inventoryCell - 1);
        if (tempGO == null) return; 
        NetworkServer.Spawn(Instantiate(tempGO));
    }

    [Command]
    private void CmdHandleInventoryInput(int inventoryCell)
    {
        HandleInventoryInput(inventoryCell);
    }
}
