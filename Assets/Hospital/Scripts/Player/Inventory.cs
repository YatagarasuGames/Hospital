using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    [SerializeField] private SyncList<byte> _inventory = new SyncList<byte>();
    private int itemsInInventory = 3;

    public bool CanCollectItem() => _inventory.Count < itemsInInventory;

    public void Add(byte itemByteId)
    {
        _inventory.Add(itemByteId);
    }

    public GameObject Get(int inventoryCell)
    {
        GameObject covertedGameObject;
        try
        {
            covertedGameObject = ItemTypeToPrefabConverter.Instance.Convert((ItemType)_inventory[inventoryCell]);
        }
        catch (ArgumentOutOfRangeException)
        {
            Debug.LogWarning("Inventory covertation in GET method was called with out of range index");
            return null;
        }
        _inventory.Remove(_inventory[inventoryCell]);
        return covertedGameObject;

    }


}
