using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CollectableItem : NetworkBehaviour
{
    [field: SerializeField] public ItemAffiliation Affiliation { get; private set; }
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public GameObject InHandPrefab { get; private set; }

    [Server]
    public void Collect()
    {
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }

    [Command]
    public void CmdCollect()
    {
        Collect();
    }
}
