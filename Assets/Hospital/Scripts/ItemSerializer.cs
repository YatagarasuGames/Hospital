using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSerializer : NetworkBehaviour
{
    public static ItemSerializer Instance;
    [SerializeField] private List<GameObject> items;
    private List<byte> itemsByteIndexes = new List<byte>();

    private void Awake()
    {
        Instance ??= this;

        for (byte i = 0; i<items.Count; i++)
        {
            itemsByteIndexes.Add(i);
        }
    }

    public byte WriteCollectableItem(NetworkWriter writer, CollectableItem item)
    {
        for(byte i = 0; i<items.Count; i++)
        {
            if (items[i].GetComponent<CollectableItem>().Type == item.Type)
            {
                writer.WriteByte(itemsByteIndexes[i]);
                return i;
            }
        }
        throw new Exception("Item was not wrote");
    }

    public CollectableItem ReadItem(NetworkReader reader)
    {
        byte type = reader.ReadByte();
        return items[type].GetComponent<CollectableItem>();
    }

}
