using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyDataEntry : MonoBehaviour
{
    public CSteamID lobbyID;
    public string LobbyName;
    public string Connections;
    [SerializeField] private TMP_Text _lobbyNameText;
    [SerializeField] private TMP_Text _connectionsText;

    public void SetLobbyData()
    {
        if (LobbyName == "") _lobbyNameText.text = "Empty";
        else _lobbyNameText.text = LobbyName;

        _connectionsText.text = Connections;
    }

    public void JoinLobby()
    {
        SteamLobby.Instance.JoinLobby(lobbyID);
    }
}
