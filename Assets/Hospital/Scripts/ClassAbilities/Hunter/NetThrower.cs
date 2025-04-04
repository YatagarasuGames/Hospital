using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NetThrower : NetworkBehaviour
{
    [SerializeField] private GameObject _throwedNet;
    [SerializeField] private float _throwForce = 150;
    private Transform _camera;

    private void OnEnable()
    {
        _camera = transform.parent.GetComponent<ModulesObjectsBuffer>().Camera.transform;
    }

    private void Update()
    {
        if (_throwedNet == null) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isServer) ThrowNet();
            else CmdThrowNet();
        }
    }

    [Server]
    private void ThrowNet()
    {
        if (isServer) print("Throw net on sercer");
        GameObject throwedNet = Instantiate(_throwedNet, _camera.transform.position + _camera.forward * 5, Quaternion.identity);
        NetworkServer.Spawn(throwedNet);
        throwedNet.GetComponent<ThrowingNet>().Init(_camera.forward * _throwForce);
    }

    [Command]
    private void CmdThrowNet()
    {
        ThrowNet();
    }
}
