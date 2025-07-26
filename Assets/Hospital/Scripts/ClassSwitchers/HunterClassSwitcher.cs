using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterClassSwitcher : BaseClassSwitcher
{
    private void Awake()
    {

    }

    [Server]
    public override void AppendClass(uint callerNetId)
    {
        base.AppendClass(callerNetId);
        if (NetworkServer.spawned.TryGetValue(callerNetId, out NetworkIdentity playerIdentity))
        {
            Inventory inventory = playerIdentity.GetComponent<Inventory>();
            if (inventory == null) return;
            inventory.Add((byte)ItemType.axe);
        }
    }
    [Server]
    public override void RemoveClass()
    {
        base.RemoveClass();
        if (NetworkServer.spawned.TryGetValue(_classOwner, out NetworkIdentity playerIdentity))
        {
            Inventory inventory = playerIdentity.GetComponent<Inventory>();
            if (inventory == null) return;
            inventory.Remove(ItemType.axe);
        }
    }
}
