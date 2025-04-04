using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasTrap : NetworkBehaviour
{
    [SyncVar] private bool _isActivated = false;
    private PlayerMovement _player;
    [SerializeField] private float _speedDebuff;
    [SerializeField] private float _duration = 5f;
    [SerializeField] private ParticleSystem _particleSystem;

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (_isActivated) return;
        if (other.gameObject.CompareTag("Survivor"))
        {
            _isActivated = true;
            _player = other.gameObject.GetComponent<PlayerMovement>();
            _player.AddSpeedDebuff(_speedDebuff, true, _duration);
            _particleSystem.Play();
            RpcEnableParticleSystem();

        }
    }

    [ClientRpc]
    private void RpcEnableParticleSystem() => _particleSystem.Play();

}
