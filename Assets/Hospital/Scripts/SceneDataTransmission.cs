using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using AYellowpaper.SerializedCollections;
using UnityEngine.SceneManagement;

public class SceneDataTransmission : NetworkBehaviour
{
    public static SceneDataTransmission Instance;

    [SerializedDictionary("Class Type", "Class SO")]
    public SerializedDictionary<ClassType, ClassData> _classesData;

    private SyncDictionary<string, ClassType> _playersClasses = new SyncDictionary<string, ClassType>();

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [Server]
    public void ChangePlayerClassData(string playerNick, ClassType playerClassType)
    {
        if (_playersClasses.ContainsKey(playerNick)) _playersClasses[playerNick] = playerClassType;
        else _playersClasses.Add(playerNick, playerClassType);
    }

    #region Player data loading

    [Server]
    private IEnumerator LoadPlayerData(NetworkConnectionToClient playerConnection)
    {
        while (playerConnection.identity == null) yield return null;

        var playerIdentity = playerConnection.identity;
        PlayerNicknameLoader playerNicknameLoader = playerIdentity.GetComponent<PlayerNicknameLoader>();

        while (playerNicknameLoader.Nickname == null) yield return null;

        ClassData classData = _classesData[_playersClasses[playerNicknameLoader.Nickname]];
        ApplyPlayerData(playerIdentity, classData);
        SkinChanger.Instance.ChangeSkin(classData, playerIdentity);
    }

    [Server]
    private void ApplyPlayerData(NetworkIdentity playerIdentity, ClassData classData)
    {
        Transform modulesGameObject = playerIdentity.gameObject.transform.Find("Modules").transform;
        playerIdentity.gameObject.tag = classData.Tag;
        playerIdentity.GetComponent<Inventory>().ItemsInInventory = classData.InentorySlots;

        if (classData.Tag == "Hunter") AddWeaponToHunter(playerIdentity);

        List<GameObject> createdModules = new List<GameObject>();
        foreach (GameObject module in classData.Modules)
        {
            GameObject tempModule = Instantiate(module, modulesGameObject.transform);
            NetworkServer.Spawn(tempModule, playerIdentity.gameObject);
            tempModule.GetComponent<Module>().SetModulesObjectsBuffer(playerIdentity.gameObject);
            createdModules.Add(tempModule);

        }

        RpcParentModules(playerIdentity.netId, createdModules.ToArray());
    }

    [Server]
    private void AddWeaponToHunter(NetworkIdentity playerIdentity)
    {
        Inventory inventory = playerIdentity.GetComponent<Inventory>();
        if (inventory == null) return;
        inventory.Add((byte)ItemType.axe);
    }

    [ClientRpc]
    private void RpcParentModules(uint playerId, GameObject[] modules)
    {
        if (NetworkClient.spawned.TryGetValue(playerId, out NetworkIdentity playerIdentity))
        {
            Transform modulesGameObject = playerIdentity.gameObject.transform.Find("Modules").transform;
            foreach (GameObject module in modules)
            {
                module.transform.SetParent(modulesGameObject);
            }
        }

    }
    #endregion


    [Server]
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene")
        {
            ClearPlayerClassesData();
            return;
        }
        if (scene.name == "Menu") Destroy(gameObject);
        foreach (var conn in NetworkServer.connections.Values) StartCoroutine(LoadPlayerData(conn));
    }

    [Server]
    private void ClearPlayerClassesData()
    {
        _playersClasses.Clear();
    }

    private void OnDisable()    
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
