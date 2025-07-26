using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDesk : NetworkBehaviour
{
    [SerializeField][SyncVar] private bool _isAttached = true;
    private Rigidbody _rigidBody;

    private void OnEnable()
    {
        _rigidBody ??= GetComponent<Rigidbody>();
        _rigidBody.isKinematic = true;
    }

    [Server]
    public void Deattach()
    {
        if (!_isAttached) return;
        _isAttached = false;
        _rigidBody.isKinematic = _isAttached;
        RpcDeattach();
    }

    [ClientRpc]
    private void RpcDeattach()
    {
        _rigidBody.isKinematic = _isAttached;
    }
}
