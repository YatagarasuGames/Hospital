using Mirror;
using UnityEngine;

public class TrapPlacer : ActiveModule
{
    [SerializeField] private GameObject _trap;
    private Transform _camera;

    [Server]
    protected override void Use()
    {
        base.Use();
        GameObject spawnedTrap = Instantiate(_trap, _camera.transform.localPosition + transform.forward * 2, Quaternion.identity);
        NetworkServer.Spawn(spawnedTrap); 
    }

    protected override void OnBufferEnabled(GameObject old, GameObject newBuffer)
    {
        var tempBuffer = _player.GetComponentInChildren<ModulesObjectsBuffer>();
        _camera = tempBuffer.Camera.transform;
    }
}