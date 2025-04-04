using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AthleteGasVulnerebility : NetworkBehaviour
{
    private PlayerMovement _playerMovement;
    private NetworkIdentity _playerNetworkIdentity;
    [SerializeField] private float _speedMultiplier = 0.5f;

    private void OnEnable()
    {
        _playerMovement = GetComponentInParent<ModulesObjectsBuffer>().Player.GetComponent<PlayerMovement>();
        _playerNetworkIdentity = GetComponentInParent<ModulesObjectsBuffer>().Player.GetComponent<NetworkIdentity>();

        if (_playerNetworkIdentity.isServer) PlayerMovement.onSpeedDebuff += AppendDoubleDebuff;
        else PlayerMovement.onSpeedDebuff += CmdAppendDoubleDebuff;
    }

    [Server]
    private void AppendDoubleDebuff(float debuff, float duration)
    {
        AddDebuff(debuff*_speedMultiplier, duration);
    }

    [Command]
    private void CmdAppendDoubleDebuff(float debuff, float duration)
    {
        CmdAddDebuff(debuff * _speedMultiplier, duration);
    }

    [Server]
    private void AddDebuff(float debuff, float duration)
    {
        _playerMovement.AddSpeedDebuff(debuff, duration: duration);
    }

    [Command]
    private void CmdAddDebuff(float debuff, float duration)
    {
        AddDebuff(debuff, duration);
    }

    




    private void OnDisable()
    {
        if (_playerNetworkIdentity.isServer) PlayerMovement.onSpeedDebuff -= AppendDoubleDebuff;
        else PlayerMovement.onSpeedDebuff -= CmdAppendDoubleDebuff;
    }


}
