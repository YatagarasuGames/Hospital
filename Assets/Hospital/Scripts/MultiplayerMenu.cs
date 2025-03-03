using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class MultiplayerMenu : MonoBehaviour
{
    [SerializeField] private NetworkManager _networkManager;
    [SerializeField] private InputField _IPinputField;
    //[SerializeField] private InputField _portInputField;

    public void StartHost()
    {
        _networkManager.StartHost();
    }

    public void StartClient()
    {
        _networkManager.networkAddress = _IPinputField.text;
        /*if (Transport.active is PortTransport portTransport)
        {
            if (ushort.TryParse(_portInputField.ToString(), out ushort port))
                portTransport.Port = port;
        }*/
        _networkManager.StartClient();

    }
}
