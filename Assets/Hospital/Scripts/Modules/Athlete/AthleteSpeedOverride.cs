using Mirror;
using UnityEngine;

public class AthleteSpeedOverride : PassiveModule
{
    [SerializeField] private float _speedBoost = 5f;
    private PlayerMovement _playerMovement;

    [Server]
    protected override void ApplyBonus()
    {
        _playerMovement.AddSpeedBuff(_speedBoost);
    }

    [Server]
    protected override void RemoveBonus()
    {
        _playerMovement.AddSpeedDebuff(_speedBoost);
    }

    protected override void OnBufferEnabled(GameObject old, GameObject newBuffer)
    {
        var tempBuffer = _player.GetComponentInChildren<ModulesObjectsBuffer>();
        _playerNetworkIdentity = tempBuffer.Player.GetComponent<NetworkIdentity>();
        _playerMovement = tempBuffer.Player.GetComponent<PlayerMovement>();

        if (isServer) ApplyBonus();
    }
}
