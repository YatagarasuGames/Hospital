using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseClassSwitcher : NetworkBehaviour
{
    [SerializeField] protected Mesh _classCharacterMesh;
    [SerializeField] protected List<GameObject> _modules;

    [Server]
    public virtual void Append(uint callerNetId)
    {
        if (NetworkServer.spawned.TryGetValue(callerNetId, out NetworkIdentity playerIdentity))
        {
            Transform modulesGameObject = playerIdentity.gameObject.transform.Find("Modules").transform;
            foreach (GameObject module in _modules)
            {
                NetworkServer.Spawn(Instantiate(module, modulesGameObject.transform), playerIdentity.gameObject);
            }
        }
        else
        {
            Debug.LogError($"Player with netId {callerNetId} not found.");
        }
    }

    [Server]
    public virtual void Remove(uint callerNetId)
    {
        if (NetworkServer.spawned.TryGetValue(callerNetId, out NetworkIdentity playerIdentity))
        {
            Transform modulesGameObject = playerIdentity.gameObject.transform.Find("Modules").transform;
            for(int i = 0; i < _modules.Count; i++)
            {
                Destroy(modulesGameObject.GetChild(0).gameObject);
            }
        }
        else
        {
            Debug.LogError($"Player with netId {callerNetId} not found.");
        }
    }
}
