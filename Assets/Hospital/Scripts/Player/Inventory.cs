using Mirror;
using System;
using UnityEngine;


[HideInInspector]
public class Inventory : NetworkBehaviour
{
    [SerializeField] private readonly SyncList<byte> _inventory = new SyncList<byte>();
    [SyncVar] public int ItemsInInventory = 2;

    public bool CanCollectItem() => _inventory.Count < ItemsInInventory;

    [Server]
    public void Add(byte itemByteId)
    {
        _inventory.Add(itemByteId);
        print($"{(ItemType)itemByteId} added");
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
        
        return covertedGameObject;

    }

    [Server]
    public void Remove(byte inventoryCell)
    {
        _inventory.Remove(_inventory[inventoryCell]);
        
    }

    [Server]
    public bool Remove(ItemType itemType)
    {
        foreach (byte b in _inventory) Debug.LogWarning(b);
        byte itemByteId = (byte)itemType;
        for(int i = 0; i< _inventory.Count; i++)
        {
            if(_inventory[i] == itemByteId)
            {
                print($"{itemType} removed");
                Remove((byte)i);
                foreach(byte b in _inventory) print(b);
                return true;
            }
        }
        return false;
    }


}
