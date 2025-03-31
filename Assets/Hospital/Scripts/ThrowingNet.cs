using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ThrowingNet : NetworkBehaviour
{
    [SyncVar] private Vector3 _throwForce;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject _attachedNet;

    [Server]
    public void Init(Vector3 throwDirection)
    {
        _throwForce = throwDirection;
        if (!isServer) return;
        _rb.AddForce(_throwForce, ForceMode.Impulse);
    }

    [Server]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Survivor"))
        {
            print(isServer);
            other.gameObject.GetComponent<PlayerMovement>().SetTrappedState(true);
            GameObject tempNet = Instantiate(_attachedNet);
            tempNet.transform.position = other.transform.position;
            NetworkServer.Spawn(tempNet, other.gameObject);
            tempNet.GetComponent<AttachedNet>().Init(other.GetComponent<NetworkIdentity>().netId);
            
        }

        Destroy(gameObject);
    }

}
