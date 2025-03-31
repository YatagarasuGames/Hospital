using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AthleteGasVulnerebility : NetworkBehaviour
{
    private PlayerMovement _playerMovement;
    [SerializeField] private float _speedMultiplier = 2f;
    private void OnEnable()
    {
        if (isOwned) print("Owned");
        _playerMovement = GetComponentInParent<ModulesObjectsBuffer>().Player.GetComponent<PlayerMovement>();
        _playerMovement.speedOverrides.OnAdd += AddSpeed;
        //_playerMovement.speedOverrides.OnRemove += RemoveSpeed;
    }

    private void AddSpeed(int indexWhereNewSpeedAdded)
    {
        _playerMovement.speedOverrides.Add(_playerMovement.speedOverrides[indexWhereNewSpeedAdded]*_speedMultiplier);
    }

    private void RemoveSpeed(int indexWhereSpeedRemoved, float removedSpeedValue)
    {

    }

    private void OnDisable()
    {
        _playerMovement.speedOverrides.OnAdd -= AddSpeed;
    }


}
