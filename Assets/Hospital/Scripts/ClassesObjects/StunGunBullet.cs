using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGunBullet : NetworkBehaviour
{
    [SyncVar] private Vector3 _throwForce;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _stunDuration;

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
            other.gameObject.GetComponent<PlayerMovement>().SetTrappedState(true, _stunDuration);
        }

        Destroy(gameObject);
    }
}
