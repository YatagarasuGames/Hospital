using Mirror;
using UnityEngine;

public class MotionSensorPlacer : ActiveModule
{
    [SerializeField] private GameObject _motionSensor;
    private Transform _camera;

    [Server]
    protected override void Use()
    {
        base.Use();
        GameObject spawnedTrap;

        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, 2f))
        {
            Quaternion rotation = Quaternion.LookRotation(hit.normal);
            spawnedTrap = Instantiate(_motionSensor, hit.point, rotation);
        }
        else
        {
            Physics.Raycast(_player.transform.position, -_player.transform.up, out RaycastHit floorHit, 4f);
            Quaternion rotation = Quaternion.LookRotation(floorHit.normal);
            spawnedTrap = Instantiate(_motionSensor, floorHit.point, rotation);
        }

        NetworkServer.Spawn(spawnedTrap);
    }



    protected override void OnBufferEnabled(GameObject old, GameObject newBuffer)
    {
        var tempBuffer = _player.GetComponentInChildren<ModulesObjectsBuffer>();
        _camera = tempBuffer.Camera.transform;
    }
}
