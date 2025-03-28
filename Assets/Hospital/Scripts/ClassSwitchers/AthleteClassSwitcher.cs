using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AthleteClassSwitcher : BaseClassSwitcher
{
    [SerializeField] private GameObject _speedBuff;
    [Server]
    public override void Append(uint playerNetId)
    {
        // ������� ������ �� ��� netId
        if (NetworkServer.spawned.TryGetValue(playerNetId, out NetworkIdentity playerIdentity))
        {

            // ��������� ��������� TrapPlacer
            NetworkServer.Spawn(Instantiate(_speedBuff, playerIdentity.gameObject.transform), playerIdentity.gameObject);

            //trapPlacer.Init(); // �������������� ������ �������
            Debug.Log($"Added SpeedBuff to player {playerIdentity.gameObject.name}");
        }
        else
        {
            Debug.LogError($"Player with netId {playerNetId} not found.");
        }
    }

    [Server]
    public override void Remove(uint playerNetId)
    {
        // ������� ������ �� ��� netId
        if (NetworkServer.spawned.TryGetValue(playerNetId, out NetworkIdentity playerIdentity))
        {
            // ������� ��������� TrapPlacer
            Destroy(playerIdentity.gameObject.GetComponentInChildren<AthleteSpeedOverride>().gameObject);
            Debug.Log($"Removed SpeedBuff from player {playerIdentity.gameObject.name}");
        }
        else
        {
            Debug.LogError($"Player with netId {playerNetId} not found.");
        }
    }
}
