using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    [SerializeField] private SyncList<byte> _inventory = new SyncList<byte>();

    public void Add(byte itemByteId)
    {
        _inventory.Add(itemByteId);
    }

    public GameObject Get(int inventoryCell)
    {

        return ItemTypeToPrefabConverter.Instance.Convert((ItemType)_inventory[inventoryCell]);
    }


}
