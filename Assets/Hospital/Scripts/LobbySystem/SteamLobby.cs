using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using System.Security.Cryptography;
using UnityEngine.UI;
using TMPro;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance;

    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;


    protected Callback<LobbyMatchList_t> LobbyList;
    protected Callback<LobbyDataUpdate_t> LobbyDataUpdated;

    public List<CSteamID> lobbyIDs = new List<CSteamID>();

    public ulong CurrentLobbyId;
    private const string HostAddressKey = "HostAddress";
    private const string gameLobbyPrefix = "HospitalYG";
    private NetworkManager networkManager;

    [Header("Lobby parameters")]
    [SerializeField] private TMP_InputField _lobbyName;
    [SerializeField] private Toggle _lobbyPrivacy;
    private void Awake()
    {
        if(Instance == null) Instance = this;
    }

    private void Start()
    {
        networkManager = NetworkManager.singleton;

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);


        LobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyData);
    }

    public void HostLobby()
    {
        if(_lobbyPrivacy.isOn) SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, networkManager.maxConnections);
        else SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, networkManager.maxConnections);
        
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) return;

        networkManager.StartHost();

        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey, SteamUser.GetSteamID().ToString());
        SetLobbyData(callback);
    }

    private void SetLobbyData(LobbyCreated_t callback)
    {
        string newLobbyName;
        if (_lobbyName.text == string.Empty) newLobbyName = $"{gameLobbyPrefix}Hospital of {SteamFriends.GetPersonaName()}";
        else newLobbyName = $"{gameLobbyPrefix}" + _lobbyName.text;
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name", newLobbyName);
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "isGameStarted", "false");
        
        print(newLobbyName);
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "maxConnections", networkManager.maxConnections.ToString());

        if (_lobbyPrivacy.isOn)
        {
            int lobbyPassword = Random.Range(100_000, 999_999);
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "password", lobbyPassword.ToString());
            print(lobbyPassword.ToString());
        }

    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        CurrentLobbyId = callback.m_ulSteamIDLobby;

        if (NetworkServer.active) return;
        networkManager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);

        networkManager.StartClient();
    }

    public void JoinLobby(CSteamID lobbyID)
    {
        if (SteamMatchmaking.GetLobbyData(lobbyID, "password") != string.Empty) { print("PasswordLobby"); return; }
        if (SteamMatchmaking.GetNumLobbyMembers(lobbyID) >= NetworkManager.singleton.maxConnections) { print("Full lobby"); return; }
        SteamMatchmaking.JoinLobby(lobbyID);
        print("Success");
    }


    public void GetLobbiesList()
    {
        if(lobbyIDs.Count > 0) lobbyIDs.Clear();
        SteamMatchmaking.AddRequestLobbyListStringFilter("name", gameLobbyPrefix, ELobbyComparison.k_ELobbyComparisonGreaterThan);
        SteamMatchmaking.RequestLobbyList();
    }

    private void OnGetLobbyList(LobbyMatchList_t result)
    {
        if (LobbiesListManager.instance.listOfLobbies.Count > 0) LobbiesListManager.instance.DestroyLobbies();

        for(int i = 0; i<result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDs.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID);
        }
    }

    private void OnGetLobbyData(LobbyDataUpdate_t result)
    {
        LobbiesListManager.instance.DisplayLobbies(lobbyIDs, result);
    }
}
