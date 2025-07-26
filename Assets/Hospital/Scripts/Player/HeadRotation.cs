using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotation : NetworkBehaviour, IDeadStateDependable
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _headBone;
    [SerializeField] private Transform _handBone;
    [SerializeField] private Transform _itemHolder;
    [Range(50f, 180f)]public float rotationOffset;
    [SyncVar] private bool _isActive = true;
    public Vector3 offset;

    [Server]
    public void ChangeState(bool newState)
    {
        _isActive = newState;
    }

    void LateUpdate()
    {
        if (!_isActive) return;
        transform.rotation = _camera.transform.rotation;
        _headBone.rotation = _camera.transform.rotation;
        Debug.DrawRay(_camera.position, _camera.forward);
        if(_itemHolder.childCount != 0 && !_itemHolder.GetChild(0).CompareTag("Weapon")) _handBone.rotation = _camera.transform.rotation * Quaternion.Euler(offset);  
    }
}
