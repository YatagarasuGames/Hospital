using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpeedStacks : PassiveModule
{
    private PlayerMovement _playerMovement;
    [SyncVar] private int _stackCount = 0;
    [SerializeField] private float _speedStackBoost = 0.2f;

    [Server]
    protected override void ApplyBonus()
    {
        _playerMovement.AddSpeedBuff(_speedStackBoost);
    }

    protected override void OnBufferEnabled(GameObject old, GameObject newBuffer)
    {
        _playerMovement = newBuffer.GetComponent<PlayerMovement>();
        if (isServer) PlayerHealth.OnPlayerDied += HandlePlayerDied;
    }

    [Server]
    private void HandlePlayerDied(NetworkIdentity player)
    {
        _stackCount++;
        ApplyBonus();
    }

    [Server]
    protected override void RemoveBonus()
    {
        PlayerHealth.OnPlayerDied -= HandlePlayerDied;
        _playerMovement.AddSpeedDebuff(_stackCount * _speedStackBoost);
    }
}
