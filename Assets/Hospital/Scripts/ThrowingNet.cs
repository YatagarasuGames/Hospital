using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ThrowingNet : NetworkBehaviour
{
    [SyncVar] private Vector3 _throwForce;
    [SerializeField] private Rigidbody _rb;
    [Server]
    public void Init(Vector3 throwDirection)
    {
        print(isServer);
        _throwForce = throwDirection;
        //GetComponent<Rigidbody>().AddForce(throwDirection, ForceMode.Impulse);
        //RpcInit(throwDirection);
        if (!isServer) return;
        _rb.AddForce(_throwForce, ForceMode.Impulse);
        print("AddedForce");
        RpcInit(throwDirection);
    }

    [Server]
    private void Update()
    {
        
    }

    [ClientRpc]
    private void RpcInit(Vector3 throwDirection)
    {
        _rb.AddForce(throwDirection, ForceMode.Impulse);
    }
}
