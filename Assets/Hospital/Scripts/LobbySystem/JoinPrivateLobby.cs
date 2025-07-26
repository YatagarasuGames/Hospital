using Edgegap;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinPrivateLobby : MonoBehaviour
{

    private SteamLobby _steamLobby;
    [SerializeField] private TMP_InputField _lobbyPassword;

    private void Awake()
    {
        _steamLobby = SteamLobby.Instance;
    }

    public void TryToJoin()
    {
        if(_lobbyPassword.text.Length != 6) return;
        for (int i = 0; i < _steamLobby.lobbyIDs.Count; i++)
        {
            if (SteamMatchmaking.GetLobbyData((CSteamID)_steamLobby.lobbyIDs[i].m_SteamID, "password") == _lobbyPassword.text)
            {
                SteamMatchmaking.JoinLobby((CSteamID)_steamLobby.lobbyIDs[i].m_SteamID);
                print("Attempt success");
                print(int.Parse(SteamMatchmaking.GetLobbyData((CSteamID)_steamLobby.lobbyIDs[i].m_SteamID, "maxConnections")));
            }
            print(SteamMatchmaking.GetLobbyData((CSteamID)_steamLobby.lobbyIDs[i].m_SteamID, "password"));
        }

    }
}
