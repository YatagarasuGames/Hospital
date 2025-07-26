using Mirror;
using UnityEngine;

public class ChemistGasTrapPlacerModule : ActiveModule
{
    [SerializeField] private GameObject _gasTrap;
    private Transform _camera;

    [Server]
    protected override void Use()
    {
        GameObject spawnedTrap = Instantiate(_gasTrap, _camera.transform.localPosition + transform.forward * 2, Quaternion.identity);
        NetworkServer.Spawn(spawnedTrap);

    }

    protected override void OnBufferEnabled(GameObject old, GameObject newBuffer)
    {
        var tempBuffer = _player.GetComponentInChildren<ModulesObjectsBuffer>();
        _camera = tempBuffer.Camera.transform;
    }
}
