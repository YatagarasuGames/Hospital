using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ChemistWallHackModule : NetworkBehaviour
{
    private List<NetworkOutline> _players = new List<NetworkOutline>();
    private void OnEnable()
    {
        foreach (var tempObject in NetworkServer.spawned)
        {
            if (tempObject.Value.CompareTag("Survivor"))
            {
                print(tempObject.Value.gameObject.GetComponent<NetworkOutline>());
                _players.Add(tempObject.Value.gameObject.GetComponent<NetworkOutline>());
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isOwned) return;

            if (isServer) EnableWallHack();
            else CmdEnableWallHack();
        }
    }

    private void EnableWallHack()
    {
        print(_players.Count);
        foreach (NetworkOutline playerOutline in _players)
        {
            if(playerOutline == null) continue;
            print(playerOutline);
            playerOutline._isOutlined = true;
            RpcSetOutlinedMaterial(playerOutline.GetComponent<NetworkIdentity>().netId, true);
        }
    }

    [ClientRpc]
    private void RpcSetOutlinedMaterial(uint netId, bool isOutlined)
    {
        if (NetworkServer.spawned.TryGetValue(netId, out var instance))
        {
            instance.gameObject.GetComponent<NetworkOutline>()._isOutlined = isOutlined;
            print("Added");
        }

    }

    [Command]
    private void CmdEnableWallHack()
    {
        EnableWallHack();
    }
}
