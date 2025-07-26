using Mirror;
using UnityEngine;

public class AthleteGasVulnerebility : PassiveModule
{
    private PlayerMovement _playerMovement;
    [SerializeField] private float _speedMultiplier = 0.5f;

    protected override void OnBufferEnabled(GameObject old, GameObject newBuffer)
    {
        var tempBuffer = _player.GetComponentInChildren<ModulesObjectsBuffer>();
        _playerMovement = tempBuffer.Player.GetComponent<PlayerMovement>();
        _playerNetworkIdentity = tempBuffer.Player.GetComponent<NetworkIdentity>();

        if (isServer)
        {
            ApplyBonus();
        }

    }

    [Server]
    protected override void ApplyBonus()
    {
        PlayerMovement.onSpeedDebuff += AppendDoubleDebuff;
    }

    [Command]
    public override void CmdApplyBonus()
    {
        ApplyBonus();
    }


    [Server]
    private void AppendDoubleDebuff(float debuff, float duration)
    {
        _playerMovement.AddSpeedDebuff(debuff, duration: duration);
    }

    [Server]
    protected override void RemoveBonus()
    {
        if (isServer) PlayerMovement.onSpeedDebuff -= AppendDoubleDebuff;
    }
}
