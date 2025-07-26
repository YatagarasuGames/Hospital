using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardReader : NetworkBehaviour
{
    [SerializeField][SyncVar] private bool _isUnlocked = false;
    [SerializeField] private GameObject _accessIndicator;
    [SerializeField] private Material _accessedIndicatorMaterial;

    [SerializeField] private Door _door;
    [field: SerializeField] public DoorKeyType Type { get; private set; }

    [Server]
    public void CheckCard()
    {
        if(_isUnlocked) return;
        _door.Unlock();
        _accessIndicator.GetComponent<MeshRenderer>().material = _accessedIndicatorMaterial;
        ChangeAccessIndicator();
        _isUnlocked=true;
    }

    [ClientRpc]
    private void ChangeAccessIndicator() => _accessIndicator.GetComponent<MeshRenderer>().material = _accessedIndicatorMaterial;


}
