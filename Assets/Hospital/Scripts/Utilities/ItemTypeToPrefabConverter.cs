using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTypeToPrefabConverter : NetworkBehaviour
{
    public static ItemTypeToPrefabConverter Instance;
    [SerializeField] private List<CollectableItem> _existingInHandPrefabs = new List<CollectableItem>();

    private void Awake()
    {
        Instance ??= this;
    }

    public GameObject Convert(ItemType itemType)
    {
        foreach(var item in _existingInHandPrefabs)
        {
            if(itemType == item.Type) return item.InHandPrefab;
        }
        return null;    
    }

}
