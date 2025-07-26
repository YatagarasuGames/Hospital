using System.Collections;
using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryInput : InputComponent
{
    [Header("Components")]
    [SerializeField] private PlayerUIDrawer _playerUIDrawer;
    [SyncVar][SerializeField] private Inventory _inventory;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _hand;

    [SyncVar] private GameObject _itemInHand;
    [SyncVar] private int _currentInventoryCell;
    private bool _isProcessingInput = false;

    public GameObject ItemInHand => _itemInHand;

    [Header("Utility Transforms")]
    [SerializeField] private Transform _itemInHandHolder;
    [SerializeField] private Transform _itemDropPosition;

    private string _animatorLayerWithItemHolding = "ItemHolding";
    private string _animatorLayerWithAxeHolding = "AxeHoldLayer";

    private void OnEnable()
    {
        InHandItem.onItemUsed += HandleItemUsed;
        PlayerHealth.OnPlayerDied += CmdHandlePlayerDied;
    }

    private void Update()
    {
        if (!isOwned || !_isActive) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isServer) HandleInventoryInput(0);
            else CmdHandleInventoryInput(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (isServer) HandleInventoryInput(1);
            else CmdHandleInventoryInput(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!transform.parent.CompareTag("Hunter")) return;
            if (isServer) HandleInventoryInput(2);
            else CmdHandleInventoryInput(2);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isServer) DropItem();
            else CmdDropItem();
        }
    }

    [Command]
    public void CmdHandleInventoryInput(int inventoryCell)
    {
        HandleInventoryInput(inventoryCell);
    }

    [Server]
    public void HandleInventoryInput(int inventoryCell)
    {
        if (_isProcessingInput) return;
        _isProcessingInput = true;

        GameObject itemFromInventory = _inventory.Get(inventoryCell);
        if (itemFromInventory == null)
        {
            DestroyItemInHand();
            _isProcessingInput = false;
            return;
        }

        if (_itemInHand) DestroyItemInHand();

        _currentInventoryCell = inventoryCell;
        var tempItemInHand = Instantiate(itemFromInventory, _itemInHandHolder);
        NetworkServer.Spawn(tempItemInHand, connectionToClient);
        _itemInHand = NetworkServer.spawned[tempItemInHand.GetComponent<NetworkIdentity>().netId].gameObject;

        RpcSetItemInHand(tempItemInHand);
        TargetAddLocalItemInHand(connectionToClient, _itemInHand);
        RpcChangeAnimatorLayerToHoldItem(1);

        _isProcessingInput = false;
    }

    [TargetRpc]
    private void TargetAddLocalItemInHand(NetworkConnection target, GameObject itemInHand)
    {
        if (!itemInHand) return;
        _hand.gameObject.SetActive(true);
        _playerUIDrawer.UpdateItemInHandUI(itemInHand);

    }

    [ClientRpc]
    private void RpcSetItemInHand(GameObject item)
    {
        if (item == null) return;
        item.transform.position = _itemInHandHolder.position;
        item.transform.SetParent(_itemInHandHolder);
    }

    [Server]
    private void HandleItemUsed(NetworkConnectionToClient conn)
    {
        if (connectionToClient != conn) return;
        DeleteItemOnUse();
    }

    [Server]
    private void DeleteItemOnUse()
    {
        _inventory.Remove((byte)_currentInventoryCell);
        DestroyItemInHand();
    }

    [Server]
    public void DestroyItemInHand()
    {
        if (_itemInHand == null) return;
        NetworkServer.Destroy(_itemInHand);
        Destroy(_itemInHand);
        _playerUIDrawer.ResetItemInHandUI();
        _itemInHand = null;
        _currentInventoryCell = -1;
        RpcChangeAnimatorLayerToHoldItem(0);
        TargetHideHandLocal(connectionToClient);
    }

    [TargetRpc]
    private void TargetHideHandLocal(NetworkConnectionToClient connection)
    {
        _hand.gameObject.SetActive(false);
    }

    [ClientRpc]
    private void RpcChangeAnimatorLayerToHoldItem(int value)
    {
        _animator.SetLayerWeight(_animator.GetLayerIndex(_animatorLayerWithItemHolding), value);
        if (value == 0) _animator.SetLayerWeight(_animator.GetLayerIndex(_animatorLayerWithAxeHolding), value);
    }

    [Server]
    public void DropItem()
    {
        if (_itemInHand == null) return;

        var inHandItem = _itemInHand.GetComponent<InHandItem>();
        if (inHandItem == null) return;
        if (inHandItem.DroppableItem == null) return;

        var temp = Instantiate(inHandItem.DroppableItem);
        temp.transform.position = _itemInHand.transform.position;
        _inventory.Remove((byte)_currentInventoryCell);
        DestroyItemInHand();
        NetworkServer.Spawn(temp);
    }

    [Server]
    private void DropItem(GameObject item)
    {
        if (item == null) return;

        var inHandItem = item.GetComponent<InHandItem>();
        if (inHandItem == null) return;

        if (inHandItem.DroppableItem == null) return;
        var temp = Instantiate(inHandItem.DroppableItem);
        temp.transform.position = transform.position;
        NetworkServer.Spawn(temp);
    }

    [Command]
    public void CmdDropItem() => DropItem();

    [Command]
    private void CmdHandlePlayerDied(NetworkIdentity player)
    {
        if (player != NetworkClient.localPlayer) return;
        for (int i = 0; i < _inventory.ItemsInInventory; i++)
        {
            DropItem(_inventory.Get(i));
        }
    }

    [Server]
    private void OnDisable()
    {
        InHandItem.onItemUsed -= HandleItemUsed;
        PlayerHealth.OnPlayerDied -= CmdHandlePlayerDied;
        for (int i = 0; i < _inventory.ItemsInInventory; i++)
        {
            DropItem(_inventory.Get(i));
        }
    }

}
