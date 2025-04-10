using System.Collections;
using System.Collections.Generic;
using Mirror;
using System.Globalization;
using UnityEngine;

public class MotionSensorPlacer : NetworkBehaviour
{
    [SerializeField] private GameObject _motionSensor; // ������ �������
    private Transform _camera;

    private void OnEnable()
    {
        _camera = transform.parent.GetComponent<ModulesObjectsBuffer>().Camera.transform;
    }
    private void Update()
    {

        // �� ������� ������� T ��������� �������
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
        NetworkServer.Spawn(spawnedTrap); // �������������� � ���������

    }

    [Command]
    private void CmdPlaceSensor()
    {
        // ������� ������� �� �������
        PlaceSensor();
    }
}
