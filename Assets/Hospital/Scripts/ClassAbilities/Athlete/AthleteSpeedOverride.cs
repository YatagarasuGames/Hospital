using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AthleteSpeedOverride : NetworkBehaviour
{
    [SerializeField] private float _speedBoost = 5f;
    private NetworkIdentity _playerNetworkIdentity;
    private PlayerMovement _playerMovement;
    private void OnEnable()
    {
        _playerMovement = GetComponentInParent<ModulesObjectsBuffer>().Player.GetComponent<PlayerMovement>();
        _playerNetworkIdentity = GetComponentInParent<ModulesObjectsBuffer>().Player.GetComponent<NetworkIdentity>();

        if(_playerNetworkIdentity.isServer) AddSpeedBoost();
        else CmdAddSpeedBoost();
    }

    [Server]
    private void AddSpeedBoost()
    {
        _playerMovement.AddSpeedBuff(_speedBoost);
    }

    [Command]
    private void CmdAddSpeedBoost()
    {
        AddSpeedBoost();
    }

    private void OnDisable()
    {
        if (_playerNetworkIdentity) RemoveSpeedBoost();
        else CmdRemoveSpeedBoost();
    }

    [Server]
    private void RemoveSpeedBoost()
    {
        _playerMovement.AddSpeedDebuff(_speedBoost);
    }

    [Command]
    private void CmdRemoveSpeedBoost()
    {
        AddSpeedBoost();
    }
}
