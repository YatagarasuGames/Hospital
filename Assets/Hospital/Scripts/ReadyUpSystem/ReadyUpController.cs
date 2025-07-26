using Mirror;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class ReadyUpController : NetworkBehaviour
{
    public static ReadyUpController Instance { get; private set; }
    [SerializeField] private List<ReadyUpPlayerItem> _readyUpItems;
    [SerializeField] private List<ReadyUpPlayerItem> _hunterReadyUpItems;
    private Dictionary<NetworkConnectionToClient, ulong> _playersSteamId = new Dictionary<NetworkConnectionToClient, ulong>();

    public void Start()
    {
        if (Instance == null) Instance = this;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.OnConnectedEvent += HandlePlayerConnected;
        NetworkServer.OnDisconnectedEvent += HandlePlayerDisconnected;
    }

    [Server]
    public void HandlePlayerConnected(NetworkConnectionToClient connection)
    {
        print("HandlePlayerConnected method");
        UniTask.Void(async () =>
        {
            await UniTask.WaitUntil(() => connection.isReady);
            PlayerSteamData playerSteamData = connection.identity.GetComponent<PlayerSteamData>();

            await UniTask.WaitUntil(() => playerSteamData.SteamID != 0);

            _playersSteamId.Add(connection, playerSteamData.SteamID);
            CreateNewReadyUpItem(connection.identity.netId, playerSteamData.SteamID);

            print("ALL PLAYERSSTEAMID");
            foreach (var v in _playersSteamId) print($"{v.Key} : {v.Value}");
        });
    }

    [Server]
    public void CreateNewReadyUpItem(uint userNetId, ulong userSteamId)
    {
        for (int i = 0; i < _readyUpItems.Count; i++)
        {
            if (_readyUpItems[i].IsInited)
            {
                _readyUpItems[i].Reload();
            }
            else
            {
                _readyUpItems[i].Init(userNetId, userSteamId);
                return;
            }
        }
    }

    [Server]
    public void HandlePlayerDisconnected(NetworkConnectionToClient connection)
    {
        print("Handle player disconnected in steam data");
        ulong steamId = _playersSteamId[connection];
        if (steamId == 0)
        {
            Debug.LogError("Steam id of connected player was not recieved");
            return;
        }
        print($"Get player id: {steamId}");
        for (int i = 0; i < _readyUpItems.Count; i++)
        {
            if (_readyUpItems[i].UserSteamId == steamId)
            {
                _readyUpItems[i].Clear();
                _hunterReadyUpItems[i].Clear();
                return;
            }
        }
    }

    [Server]
    public void TryStartGame(NetworkIdentity interactCaller)
    {
        if (!(interactCaller.isServer && interactCaller.isClient)) return;
        for (int i = 0; i < _playersSteamId.Count; i++)
        {
            if (_readyUpItems[i].ReadyUpButton.IsReady == false) return;
        }
        if (!CheckAllPlayersHasRole()) return;
        StartGame();
    }

    [Server]
    private bool CheckAllPlayersHasRole()
    {
        bool isHunter = false;
        foreach (var player in _playersSteamId.Keys)
        {
            if (player.identity.CompareTag("Hunter")) isHunter = true;
            if (player.identity.CompareTag("Untagged")) return false;
        }
        return isHunter;
    }

    [Server]
    private void StartGame()
    {
        SteamMatchmaking.SetLobbyData((CSteamID)SteamLobby.Instance.CurrentLobbyId, "isGameStarted", "true");
        NetworkManager.singleton.ServerChangeScene("Game");
    }

    [Server]
    public void ResetReadyState(uint playerNetId)
    {
        for(int i = 0; i < _playersSteamId.Count; i++)
        {
            if (_readyUpItems[i].UserNetId == playerNetId)
            {
                _readyUpItems[i].ReadyUpButton.ResetReadyState();
                return;
            }
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        NetworkServer.OnConnectedEvent -= HandlePlayerConnected;
        NetworkServer.OnDisconnectedEvent -= HandlePlayerDisconnected;
    }
}
