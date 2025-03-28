using System.Collections;
using System.Collections.Generic;
using Mirror;
using System.Globalization;
using UnityEngine;

public class MotionSensorPlacer : NetworkBehaviour
{
    [SerializeField] private GameObject _motionSensor; // Префаб ловушки
    private Transform _camera;
    private string _trapPrefabName = "MotionSensor"; // Имя префаба ловушки

    private void OnEnable()
    {
        _camera = transform.parent.GetComponentInChildren<Camera>().transform;
    }
    private void Update()
    {

        // По нажатию клавиши T размещаем ловушку
        if (Input.GetKeyDown(KeyCode.T))
        {
            print(isOwned);
            print(isLocalPlayer);
            if (!isOwned || _motionSensor == null) return;

            if (isServer) PlaceSensor();
            else CmdPlaceSensor();
        }
    }

    [Server]
    private void PlaceSensor()
    {
        GameObject spawnedTrap = Instantiate(_motionSensor, _camera.transform.localPosition + transform.forward * 2, Quaternion.identity);
        NetworkServer.Spawn(spawnedTrap); // Синхронизируем с клиентами

    }

    [Command]
    private void CmdPlaceSensor()
    {
        // Создаем ловушку на сервере
        PlaceSensor();
    }
}
