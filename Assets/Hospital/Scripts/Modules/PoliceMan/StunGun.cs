using Mirror;
using UnityEngine;

public class StunGun : ActiveModule
{
    [SerializeField] private GameObject _stungunBullet;
    [SerializeField] private float _throwForce = 150;
    private Transform _camera;

    protected override void OnBufferEnabled(GameObject old, GameObject newBuffer)
    {
        var tempBuffer = _player.GetComponentInChildren<ModulesObjectsBuffer>();
        _camera = tempBuffer.Camera.transform;
    }

    [Server]
    protected override void Use()
    {
        base.Use();
        GameObject shotBullet = Instantiate(_stungunBullet, _camera.transform.position + _camera.forward * 2f, Quaternion.identity);
        NetworkServer.Spawn(shotBullet);
        shotBullet.GetComponent<StunGunBullet>().Init(_camera.forward * _throwForce);
    }
}
