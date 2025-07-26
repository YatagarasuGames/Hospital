using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbiesListManager : MonoBehaviour
{
    public static LobbiesListManager instance;

    [SerializeField] private GameObject _lobbiesMenu;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _lobbyListContent;
    [SerializeField] private GameObject _lobbyDataItemPrefab;

    public List<GameObject> listOfLobbies = new List<GameObject>();
    private List<LobbyDataEntry> _lobbies = new List<LobbyDataEntry>();
    private void Awake()
    {
        if (instance == null) { instance = this; }
    }

    public void GetListOfLobbies()
    {
        _mainMenu.SetActive(false);
        _lobbiesMenu.SetActive(true);
        SteamLobby.Instance.GetLobbiesList();
    }

    public void DisplayLobbies(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result)
    {
        for(int i = 0; i < lobbyIDs.Count; i++)
        {
            print(SteamMatchmaking.GetLobbyData(lobbyIDs[i], "maxConnections"));
            if (lobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby 
                && SteamMatchmaking.GetLobbyData(lobbyIDs[i], "name").ToLower().Contains("hospitalyg")
                && SteamMatchmaking.GetLobbyData(lobbyIDs[i], "password") == string.Empty
                && SteamMatchmaking.GetLobbyData(lobbyIDs[i], "isGameStarted") == "false"
                && SteamMatchmaking.GetNumLobbyMembers(lobbyIDs[i]) < int.Parse(SteamMatchmaking.GetLobbyData(lobbyIDs[i], "maxConnections")))
            {
                
                GameObject createdItem = Instantiate(_lobbyDataItemPrefab);
                SetLobbyObject(createdItem, (CSteamID)lobbyIDs[i].m_SteamID);

                listOfLobbies.Add(createdItem);
                _lobbies.Add(createdItem.GetComponent<LobbyDataEntry>());
            }
        }
    }

    private void SetLobbyObject(GameObject lobbyObject, CSteamID lobbyId)
    {
        lobbyObject.transform.SetParent(_lobbyListContent.transform);
        lobbyObject.transform.localScale = Vector3.one;
        lobbyObject.transform.localPosition = Vector3.zero;


        LobbyDataEntry lobby = lobbyObject.GetComponent<LobbyDataEntry>();
        lobby.lobbyID = lobbyId;
        lobby.LobbyName = SteamMatchmaking.GetLobbyData(lobbyId, "name").Replace("HospitalYG", "");
        lobby.Connections = $"{SteamMatchmaking.GetNumLobbyMembers(lobbyId)} / {SteamMatchmaking.GetLobbyData(lobbyId, "maxConnections")}";
        lobby.SetLobbyData();
    }

    public void DestroyLobbies()
    {
        foreach (GameObject lobbyItem in listOfLobbies) Destroy(lobbyItem);
    }

    public void HandleSearchFieldChanged(string searchRequest)
    {
        if (searchRequest == string.Empty) foreach (var lobby in _lobbies) lobby.gameObject.SetActive(true);
        else
        {
            foreach (var lobby in _lobbies)
            {
                if (lobby.LobbyName.ToLower().Contains(searchRequest.ToLower())) lobby.gameObject.SetActive(true);
                else lobby.gameObject.SetActive(false);
            }
        }

    }


}
