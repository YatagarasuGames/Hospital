using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;

public class PlayerHealth : NetworkBehaviour, IDamagable
{
    [SerializeField][SyncVar(hook = nameof(HandleHealthChanged))] private float _health = 100;
    [SerializeField] private GameObject _spectatorCameraObject;
    [SerializeField] private Image _damageIndicator;

    public static Action<NetworkIdentity> OnPlayerDied;
    public static Action<NetworkIdentity> OnPlayerGetDamage;
    [Server]
    public void GetDamage(float damage)
    {
        if(_health <= 0) return;
        _health -= damage;
        _damageIndicator.color = new Color(_damageIndicator.color.r, _damageIndicator.color.g, _damageIndicator.color.b, 1 - _health / 100);
        if (_health <= 0) Die();
    }

    private void HandleHealthChanged(float oldHealth, float newHealth)
    {
        _damageIndicator.color = new Color(_damageIndicator.color.r, _damageIndicator.color.g, _damageIndicator.color.b, 1 - newHealth / 100);
    }

    [Server]
    private void Die()
    {
        NetworkServer.Spawn(Instantiate(_spectatorCameraObject), connectionToClient);
        RpcInvoke();
        
    }

    

    [ClientRpc]
    private void RpcInvoke()
    {
        OnPlayerDied?.Invoke(GetComponent<NetworkIdentity>());
    }
}
