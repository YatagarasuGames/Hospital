using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGameButtons : MonoBehaviour
{
    private NetworkManager _networkManager;

    private void Start()
    {
        _networkManager = NetworkManager.singleton;
    }

    public void StartHost()
    {
        _networkManager.StartHost();
    }

    public void StartClient()
    {
        _networkManager.StartClient();
    }
}
