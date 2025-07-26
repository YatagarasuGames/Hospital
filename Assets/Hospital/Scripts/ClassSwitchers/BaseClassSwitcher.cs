using Mirror;
using System.Linq;
using UnityEngine;

public abstract class BaseClassSwitcher : NetworkBehaviour, IInteract
{
    [SyncVar(hook = nameof(OnRolePickedStateChanged))] protected bool _isPicked = false;
    [SerializeField] private Material _pickedStateMaterial;
    [SerializeField] private Material _notPickedStateMaterial;
    [SerializeField] private ClassData _classData;

    [SyncVar] protected uint _classOwner = 0;
    public uint ClassOwner => _classOwner;
    [field: SerializeField] public ClassType ClassType { get; private set; }
    private readonly SyncList<GameObject> _createdModules = new SyncList<GameObject>();

    [Server]
    public void Interact(uint interactCaller)
    {
        ChangeClass(interactCaller);
    }

    [Command]
    public void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }

    [Server]
    private void ChangeClass(uint interactCaller)
    {
        if (_isPicked && _classOwner != interactCaller) return;
        if (NetworkServer.spawned.TryGetValue(interactCaller, out NetworkIdentity playerIdentity))
        {
            if(_isPicked && _classOwner == interactCaller)
            {
                playerIdentity.GetComponent<PlayerClass>().SetClass(ClassType.@default);
                _classOwner = 0;
                _isPicked = false;
            }
            else
            {
                playerIdentity.GetComponent<PlayerClass>().SetClass(ClassType);
                _classOwner = interactCaller;
                _isPicked = true;
            }

        }

    }

    [Server]
    public virtual void AppendClass(uint playerNetId)
    {
        if (NetworkServer.spawned.TryGetValue(playerNetId, out NetworkIdentity playerIdentity))
        {
            Transform modulesGameObject = playerIdentity.gameObject.transform.Find("Modules").transform;
            playerIdentity.gameObject.tag = _classData.Tag;
            playerIdentity.GetComponent<Inventory>().ItemsInInventory = _classData.InentorySlots;

            GameObject[] tempModules;
            foreach (GameObject module in _classData.Modules)
            {
                GameObject tempModule = Instantiate(module, modulesGameObject.transform);
                NetworkServer.Spawn(tempModule, playerIdentity.gameObject);
                tempModule.GetComponent<Module>().SetModulesObjectsBuffer(playerIdentity.gameObject);
                _createdModules.Add(tempModule);
                
            }
            tempModules = _createdModules.ToArray();
            RpcParentModules(playerIdentity.netId, tempModules);

            SkinChanger.Instance.ChangeSkin(_classData, playerIdentity);
            SceneDataTransmission.Instance.ChangePlayerClassData(playerIdentity.GetComponent<PlayerNicknameLoader>().Nickname, ClassType);
            ReadyUpController.Instance.ResetReadyState(playerNetId);
        }
        else
        {
            Debug.LogError($"Player with netId {playerNetId} not found.");
        }
    }

    [ClientRpc]
    private void RpcParentModules(uint playerId, GameObject[] modules)
    {
        if (NetworkClient.spawned.TryGetValue(playerId, out NetworkIdentity playerIdentity))
        {
            Transform modulesGameObject = playerIdentity.gameObject.transform.Find("Modules").transform;
            print(modules.Length);
            foreach (GameObject module in modules)
            {
                module.transform.SetParent(modulesGameObject);
            }
        }

    }

    [Server]
    public virtual void RemoveClass()
    {
        if (NetworkServer.spawned.TryGetValue(_classOwner, out NetworkIdentity playerIdentity))
        {
            SkinChanger.Instance.ResetSkin(playerIdentity);
            playerIdentity.tag = "Untagged";
        }
        _isPicked = false;
        _classOwner = 0;
        
        foreach(GameObject module in _createdModules)
        {
            NetworkServer.Destroy(module);
        }
        _createdModules.Clear();    
    }


    private void OnRolePickedStateChanged(bool oldState, bool newState)
    {
        if (newState) GetComponent<MeshRenderer>().material = _pickedStateMaterial;
        else GetComponent<MeshRenderer>().material = _notPickedStateMaterial;
    }
}
