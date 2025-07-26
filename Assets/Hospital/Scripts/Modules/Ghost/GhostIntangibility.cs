using Cysharp.Threading.Tasks;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostIntangibility : ActiveModule
{
    [SerializeField] private float _intangibilityDuration;
    protected override void OnBufferEnabled(GameObject oldPlayer, GameObject newPlayer) { }

    [Server]
    protected override void Use()
    {
        base.Use();
        _player.gameObject.layer = LayerMask.NameToLayer("Intangibility");
        RemoveIntangibility(_intangibilityDuration).Forget();
    }

    [Server]
    private async UniTask RemoveIntangibility(float delay)
    {
        await UniTask.Delay((int)delay * 1000);
        _player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        RemoveIntangibility(0).Forget();
    }
}
