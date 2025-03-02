using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juice : NetworkBehaviour, ICollectable
{
    [Server]
    public void Collect()
    {
        print("Collected");
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }

    [Command]
    public void CmdCollect()
    {
        Collect();
    }

}
