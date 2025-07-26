using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MilitaryPassiveStaminaBuff : PassiveModule
{
    private List<PlayerSprint> _players = new List<PlayerSprint>();

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.OnConnectedEvent += HandlePlayerConnected;
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name != "SampleScene")
        {
            if (!isServer) return;
            ApplyBonus();
        }
    }

    [Server]
    protected override void ApplyBonus()
    {
        foreach (var player in _players) if(!player.gameObject.CompareTag("Hunter")) player.AddSprintTimeBuff(0, 1.25f);
    }

    [Server]
    private void TargetApplyBonus(PlayerSprint playerConcentration)
    {
        playerConcentration.AddSprintTimeBuff(0, 1.25f);
    }

    protected override void OnBufferEnabled(GameObject oldPlayer, GameObject newPlayer)
    {
        if (!isServer) return;
        foreach (var player in NetworkServer.connections.Values) _players.Add(player.identity.gameObject.GetComponent<PlayerSprint>());
    }

    [Server]
    private void HandlePlayerConnected(NetworkConnectionToClient connectedPlayer)
    {
        StartCoroutine(ApplyBonusToPlayer(connectedPlayer));
    }

    [Server]
    private IEnumerator ApplyBonusToPlayer(NetworkConnectionToClient connectedPlayer)
    {
        while (connectedPlayer.identity == null) yield return null;
        PlayerSprint playerConcentration = connectedPlayer.identity.gameObject.GetComponent<PlayerSprint>();
        _players.Add(playerConcentration);
        //TargetApplyBonus(playerConcentration);
    }

    protected override void RemoveBonus()
    {
        foreach (var player in _players) if (!player.gameObject.CompareTag("Hunter")) player.RemoveSprintTimeBuff(0, 1.25f).Forget();
    }

    public override void OnStopServer()
    {
        NetworkServer.OnConnectedEvent -= HandlePlayerConnected;
        base.OnStopServer();
    }
}
