using Mirror;
using UnityEngine;

public class NetThrower : ActiveModule
{
    [SerializeField] private GameObject _throwedNet;
    [SerializeField] private float _throwForce = 150;
    private Transform _camera;

    [Server]
    protected override void Use()
    {
        base.Use();
        if (isServer) print("Throw net on sercer");
        GameObject throwedNet = Instantiate(_throwedNet, _camera.transform.position + _camera.forward * 5, Quaternion.identity);
        NetworkServer.Spawn(throwedNet);
        throwedNet.GetComponent<ThrowingNet>().Init(_camera.forward * _throwForce);
    }

    protected override void OnBufferEnabled(GameObject old, GameObject newBuffer)
    {
        var tempBuffer = _player.GetComponentInChildren<ModulesObjectsBuffer>();
        _camera = tempBuffer.Camera.transform;
    }
}
