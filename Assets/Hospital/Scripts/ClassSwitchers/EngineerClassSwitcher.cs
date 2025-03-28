using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class EngineerClassSwitcher : BaseClassSwitcher
{
    [SerializeField] private GameObject _motionSensorPlacerModule;
    [Server]
    public override void Append(uint playerNetId)
    {
        // ������� ������ �� ��� netId
        if (NetworkServer.spawned.TryGetValue(playerNetId, out NetworkIdentity playerIdentity))
        {

            // ��������� ��������� TrapPlacer
            NetworkServer.Spawn(Instantiate(_motionSensorPlacerModule, playerIdentity.gameObject.transform), playerIdentity.gameObject);

            //trapPlacer.Init(); // �������������� ������ �������
            Debug.Log($"Added TrapPlacer to player {playerIdentity.gameObject.name}");
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
            Destroy(playerIdentity.gameObject.GetComponentInChildren<MotionSensorPlacer>().gameObject);
            Debug.Log($"Removed TrapPlacer from player {playerIdentity.gameObject.name}");
        }
        else
        {
            Debug.LogError($"Player with netId {playerNetId} not found.");
        }
    }
}
