using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CollectableItem : NetworkBehaviour
{
    [field: SerializeField] public ItemAffiliation Affiliation { get; private set; }
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public GameObject InHandPrefab { get; private set; }


    public override void OnStartServer()
    {
        Debug.Log($"Item spawned on server: {gameObject.name}");
    }

    public override void OnStartClient()
    {
        Debug.Log($"Item spawned on client: {gameObject.name}");
    }

    [Server]
    public void Collect()
    {
        Debug.Log("Collecting item on server: " + gameObject.name);
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }
}
