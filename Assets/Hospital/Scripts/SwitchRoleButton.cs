using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Reflection;

public class SwitchRoleButton : NetworkBehaviour, IInteract
{
    [SyncVar] public bool _wasClicked = false;
    [SerializeField] private string newSpawnpointPositionName;
    private Vector3 _newSpawnpointPosition;

    private void OnEnable()
    {
        _newSpawnpointPosition = GameObject.Find(newSpawnpointPositionName).transform.position;
    }

    [Server]
    public void Interact(uint interactCaller)
    {
        if(!_wasClicked && NetworkServer.spawned.TryGetValue(interactCaller, out NetworkIdentity player))
        {
            _wasClicked = true;
            DeleteAllModulesOnPlayer(interactCaller);
            player.GetComponent<NetworkTransformReliable>().ServerTeleport(_newSpawnpointPosition, Quaternion.identity);
        }
    }

    [Command]
    public void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }

    [Server]
    private void DeleteAllModulesOnPlayer(uint _playerId)
    {
        if (NetworkServer.spawned.TryGetValue(_playerId, out NetworkIdentity playerIdentity))
        {
            Transform modulesGameObject = playerIdentity.gameObject.transform.Find("Modules").transform;
            int modulesCount = modulesGameObject.childCount;
            print(modulesCount);
            for (int i = 0; i < modulesCount; i++)
            {
                Destroy(modulesGameObject.GetChild(i).gameObject);
                print("destroyed child");
            }
        }
    }
}
