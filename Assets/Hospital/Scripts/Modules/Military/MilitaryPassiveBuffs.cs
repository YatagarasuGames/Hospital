using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MilitaryPassiveBuffs : PassiveModule
{
    private List<PlayerConcentration> _players = new List<PlayerConcentration>();

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.OnConnectedEvent += HandlePlayerConnected;
    }

    [Server]
    protected override void ApplyBonus()
    {
        foreach (var player in _players) player.IncreaseMinConcentration(2);
    }

    [Server]
    private void TargetApplyBonus(PlayerConcentration playerConcentration)
    {
        playerConcentration.IncreaseMinConcentration(2);
    }

    protected override void OnBufferEnabled(GameObject oldPlayer, GameObject newPlayer)
    {
        if (!isServer) return;
        foreach(var player in NetworkServer.connections.Values) _players.Add(player.identity.gameObject.GetComponent<PlayerConcentration>());
        ApplyBonus();
    }

    [Server]
    private void HandlePlayerConnected(NetworkConnectionToClient connectedPlayer)
    {
        StartCoroutine(ApplyBonusToPlayer(connectedPlayer));
    }

    [Server]
    private IEnumerator ApplyBonusToPlayer(NetworkConnectionToClient connectedPlayer)
    {
        while(connectedPlayer.identity == null) yield return null;
        PlayerConcentration playerConcentration = connectedPlayer.identity.gameObject.GetComponent<PlayerConcentration>();
        _players.Add(playerConcentration);
        TargetApplyBonus(playerConcentration);
    }

    protected override void RemoveBonus()
    {
        foreach (var player in _players) player.DecreaseMinConcentration(2);
    }

    public override void OnStopServer()
    {
        NetworkServer.OnConnectedEvent -= HandlePlayerConnected;
        base.OnStopServer();
    }
}
