using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSteamData : NetworkBehaviour
{
    public static Action<uint, ulong> OnSteamIDRecieved;
    [SyncVar(hook = nameof(OnSteamIDUpdated))]
    private ulong _steamID;
    public ulong SteamID => _steamID;

    private void Start()
    {
        if (!isLocalPlayer) return;
        GetSteamID();
    }

    private void GetSteamID()
    {
        if (SteamManager.Initialized)
        {
            CSteamID id = SteamUser.GetSteamID();
            CmdSetSteamIDOnServer(id.m_SteamID);
        }
    }

    [Command]
    private void CmdSetSteamIDOnServer(ulong steamID)
    {
        _steamID = steamID;
    }


    private void OnSteamIDUpdated(ulong oldID, ulong newID)
    {
        Debug.Log($"SteamID updated: {newID}");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer)
        {
            Debug.Log($"Player connected with SteamID: {_steamID}");
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        //ReadyUpController.Instance.HandlePlayerDisconnected(_steamID);
    }
}
