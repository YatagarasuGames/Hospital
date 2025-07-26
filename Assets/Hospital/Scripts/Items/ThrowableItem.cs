using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ThrowableItem : NetworkBehaviour
{
    [SyncVar] private bool _hasHitPlayer = false;
    [SerializeField] private float _debuffDuration;
    [SerializeField] private float _debuffValue;

    [Server]
    private void OnCollisionEnter(Collision collision)
    {
        if (_hasHitPlayer) return;
        if (collision.gameObject.CompareTag("Hunter"))
        {
            _hasHitPlayer = true;
            collision.gameObject.GetComponent<PlayerMovement>().AddSpeedDebuff(_debuffValue, false, _debuffDuration);
        }
    }
}
