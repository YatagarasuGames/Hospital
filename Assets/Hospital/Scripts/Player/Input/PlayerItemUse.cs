using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerItemUse : InputComponent
{
    [SerializeField] private PlayerInventoryInput _inventoryInput;

    private void Update()
    {
        if(!isOwned || !_isActive) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isServer) Use();
            else CmdUse();
        }
    }

    [Command]
    private void CmdUse() => Use();

    [Server]
    private void Use()
    {
        if(_inventoryInput.ItemInHand == null) return;
        _inventoryInput.ItemInHand.GetComponent<InHandItem>().Use();
    }
}
