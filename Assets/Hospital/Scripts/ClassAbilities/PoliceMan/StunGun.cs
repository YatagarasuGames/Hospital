using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGun : NetworkBehaviour
{
    [SerializeField] private GameObject _stungunBullet;
    [SerializeField] private float _throwForce = 150;
    private Transform _camera;

    private void OnEnable()
    {
        _camera = transform.parent.GetComponent<ModulesObjectsBuffer>().Camera.transform;
    }

    private void Update()
    {
        if (_stungunBullet == null || !isOwned) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isServer) Shoot();
            else CmdShoot();
        }
    }

    [Server]
    private void Shoot()
    {
        GameObject shotBullet = Instantiate(_stungunBullet, _camera.transform.position + _camera.forward * 2f, Quaternion.identity);
        NetworkServer.Spawn(shotBullet);
        shotBullet.GetComponent<StunGunBullet>().Init(_camera.forward * _throwForce);
    }

    [Command]
    private void CmdShoot()
    {
        Shoot();
    }
}
