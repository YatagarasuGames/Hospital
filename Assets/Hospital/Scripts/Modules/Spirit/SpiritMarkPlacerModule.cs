using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritMarkPlacerModule : ActiveModule
{
    [SerializeField] private GameObject _spiritMarkPrefab;
    private Transform _camera;
    [SyncVar] private GameObject _placedSpiritMark;
    public GameObject PlacedSpiritMark => _placedSpiritMark;
    protected override void OnBufferEnabled(GameObject oldPlayer, GameObject newPlayer)
    {
        var tempBuffer = _player.GetComponentInChildren<ModulesObjectsBuffer>();
        _camera = tempBuffer.Camera.transform;
    }

    public override void Update()
    {
        base.Update();
        print($"Time {Time.time} ||| Next use time {_nextUseTime} ||| Can use? {Time.time > _nextUseTime}");
    }

    [Server]
    protected override void Use()
    {
        base.Use();
        if (_placedSpiritMark != null) NetworkServer.Destroy(_placedSpiritMark);
        GameObject tempMark;
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, 2f))
        {
            tempMark = Instantiate(_spiritMarkPrefab, hit.point, Quaternion.identity);
        }
        else tempMark = Instantiate(_spiritMarkPrefab, _camera.parent.position, Quaternion.identity);
        NetworkServer.Spawn(tempMark);
        _placedSpiritMark = tempMark;
    }

    public override void OnStopServer()
    {
        if(_placedSpiritMark) NetworkServer.Destroy(_placedSpiritMark);
        base.OnStopServer();
    }
}
