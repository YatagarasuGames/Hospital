using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkManagerCloneRemover : MonoBehaviour
{
    [SerializeField] private GameObject _networkManager;

    private void Awake()
    {
        if(NetworkManager.singleton == null) Instantiate(_networkManager);
    }
}
