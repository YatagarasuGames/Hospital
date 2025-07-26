using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SingletonCreator : NetworkBehaviour
{
    [SerializeField] private GameObject _singleton;
    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.Spawn(Instantiate(_singleton));
    }
}
