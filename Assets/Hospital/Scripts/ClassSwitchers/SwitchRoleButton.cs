using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Reflection;
using System.Linq;
using UnityEngine.Events;

public class SwitchRoleButton : BaseClassSwitcher
{
    //[SyncVar] public bool _wasClicked = false;
    //[SerializeField] private Transform _newSpawnpointPosition;
    //[SerializeField] private UnityEvent _onAxeDestroyed;

    //[Server]
    //public override void Append(uint callerNetId)
    //{
    //    if (!_wasClicked && NetworkServer.spawned.TryGetValue(callerNetId, out NetworkIdentity player))
    //    {
    //        player.GetComponent<NetworkTransformReliable>().ServerTeleport(_newSpawnpointPosition.position, Quaternion.identity);
    //        DeleteAllModulesOnPlayer(callerNetId);
    //        base.Append(callerNetId);
    //        if (NetworkServer.spawned.TryGetValue(callerNetId, out NetworkIdentity playerIdentity))
    //        {
    //            Inventory inventory = playerIdentity.GetComponent<Inventory>();
    //            if (inventory == null) return;
    //            if (inventory.Remove(ItemType.axe)) playerIdentity.GetComponentInChildren<PlayerInventoryInput>().DestroyItemInHand();
                
    //        }
    //    }
    //}

    //[Server]
    //private void DeleteAllModulesOnPlayer(uint _playerId)
    //{
    //    if (NetworkServer.spawned.TryGetValue(_playerId, out NetworkIdentity playerIdentity))
    //    {
    //        Transform modulesGameObject = playerIdentity.gameObject.transform.Find("Modules").transform;
    //        int modulesCount = modulesGameObject.childCount;
    //        print(modulesCount);
    //        for (int i = 0; i < modulesCount; i++)
    //        {
    //            Destroy(modulesGameObject.GetChild(i).gameObject);
    //            print("destroyed child");
    //        }

    //    }
    //}

    /*[Server]
    public void Interact(uint interactCaller)
    {
        print("Button clicked");
        print(interactCaller);
        print(NetworkServer.spawned.TryGetValue(interactCaller, out NetworkIdentity p));
        if (!_wasClicked && NetworkServer.spawned.TryGetValue(interactCaller, out NetworkIdentity player))
        {
            //_wasClicked = true;
            DeleteAllModulesOnPlayer(interactCaller);
            player.GetComponent<NetworkTransformReliable>().ServerTeleport(_newSpawnpointPosition.position, Quaternion.identity);
            player.gameObject.AddComponent<TestScript>();
            TargetAddComponent(interactCaller);
            SetDefault(interactCaller) ;
            
        }
    }

    [ClientRpc]
    private void TargetAddComponent(uint playerId)
    {
        if (!_wasClicked && NetworkServer.spawned.TryGetValue(playerId, out NetworkIdentity player))
        {
            player.gameObject.AddComponent<TestScript>();
        }
    }

    [Command]
    public void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }



    [Server]
    public virtual void SetDefault(uint callerNetId)
    {
        if (NetworkServer.spawned.TryGetValue(callerNetId, out NetworkIdentity playerIdentity))
        {
            Transform modulesGameObject = playerIdentity.gameObject.transform.Find("Modules").transform;
            playerIdentity.gameObject.tag = _defaultCharacterData.Tag;
            playerIdentity.GetComponent<Inventory>().ItemsInInventory = _defaultCharacterData.InentorySlots;
            ChangeSkin(callerNetId);
        }
        else
        {
            Debug.LogError($"Player with netId {callerNetId} not found.");
        }
    }

    [Server]
    private void ChangeSkin(uint callerNetId)
    {
        if (NetworkServer.spawned.TryGetValue(callerNetId, out NetworkIdentity playerIdentity))
        {
            playerIdentity.gameObject.tag = _defaultCharacterData.Tag;
            var meshRenderer = playerIdentity.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            meshRenderer.sharedMesh = _defaultCharacterData.Mesh;
            meshRenderer.sharedMaterials = _defaultCharacterData.Materials.ToArray();
            if (playerIdentity.connectionToClient != null && playerIdentity.connectionToClient.isReady)
            {
                RpcChangeSkin(callerNetId, _defaultCharacterData.Mesh.name, _defaultCharacterData.Materials.Select(m => m.name).ToArray());
            }
        }
    }
    [ClientRpc]
    private void RpcChangeSkin(uint callerNetId, string meshName, string[] materialNames)
    {
        print("OK");
        Mesh loadedMesh = Resources.Load<Mesh>(meshName);
        if (loadedMesh == null)
        {
            Debug.LogError($"Mesh {meshName} not found in Resources");
            return;
        }
        print("OK");
        Material[] loadedMaterials = new Material[materialNames.Length];
        for (int i = 0; i < materialNames.Length; i++)
        {
            loadedMaterials[i] = Resources.Load<Material>(materialNames[i]);
            if (loadedMaterials[i] == null)
            {
                Debug.LogError($"Material {materialNames[i]} not found in Resources");
                return;
            }
        }
        print("OK");
        if (NetworkClient.spawned.TryGetValue(callerNetId, out NetworkIdentity playerIdentity))
        {
            var meshRenderer = playerIdentity.GetComponentInChildren<SkinnedMeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.sharedMesh = loadedMesh;
                meshRenderer.sharedMaterials = loadedMaterials;
            }
        }
        print("OK");
    }*/
}
