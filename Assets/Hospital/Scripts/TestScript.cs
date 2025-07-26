using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : NetworkBehaviour
{
    protected Callback<LobbyEnter_t> LobbyEntered;

    public override void OnStartClient()
    {
        base.OnStartClient();
        print(SteamUser.GetSteamID());
    }

}
