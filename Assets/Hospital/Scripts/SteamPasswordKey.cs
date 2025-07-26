using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class SteamPasswordKey : NetworkBehaviour
{
    private void OnEnable()
    {
        SteamLobby steamLobby = FindAnyObjectByType<SteamLobby>();
        if (steamLobby == null) return;

        GetComponent<TMP_Text>().text = SteamMatchmaking.GetLobbyData(new CSteamID(steamLobby.CurrentLobbyId), "password");
    }
}
