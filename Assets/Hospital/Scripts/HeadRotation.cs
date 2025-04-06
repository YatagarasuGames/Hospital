using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRotation : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _headBone;

    void LateUpdate()
    {
        transform.rotation = _camera.transform.rotation;
        _headBone.rotation = _camera.transform.rotation;
    }
}
