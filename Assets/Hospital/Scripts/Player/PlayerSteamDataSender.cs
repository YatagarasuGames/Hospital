using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;

public class PlayerSteamDataSender : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnSteamIDUpdated))]
    public ulong steamID;

    private void Start()
    {
        if (isLocalPlayer)
        {
            // ѕолучаем SteamID только дл€ локального игрока
            GetSteamID();
        }
    }

    private void GetSteamID()
    {
        if (SteamManager.Initialized)
        {
            CSteamID id = SteamUser.GetSteamID();
            CmdSetSteamID(id.m_SteamID);
        }
    }

    [Command]
    private void CmdSetSteamID(ulong newSteamID)
    {
        // —ервер устанавливает SteamID дл€ всех клиентов
        steamID = newSteamID;
        FindObjectOfType<ReadyUpController>().CreateNewReadyUpItem(GetComponent<NetworkIdentity>().netId, newSteamID);
    }

    private void OnSteamIDUpdated(ulong oldID, ulong newID)
    {
        Debug.Log($"SteamID updated: {newID}");
        // «десь можно обновить UI или другие системы
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer)
        {
            Debug.Log($"Player connected with SteamID: {steamID}");
        }
    }
}
