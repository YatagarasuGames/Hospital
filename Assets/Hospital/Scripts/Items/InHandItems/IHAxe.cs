using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHAxe : InHandItem
{
    private readonly string _axeHoldLayer = "AxeHoldLayer";
    private readonly string _itemHoldLayer = "ItemHolding";

    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private LayerMask _attackableLayers;
    [SerializeField, Range(10, 100)] private float _damage = 40;
    private Animator _animator;

    private float _lastAttackTime;
    private bool _canAttack => Time.time - _lastAttackTime >= _attackCooldown;
    private bool _isAttacking => !_canAttack;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        if(!isOwned) return;
        if (isServer) RpcChangeAnimatorLayerToHoldItem(1);
        else CmdChangeAnimatorLayerToHoldItem(1);
        _animator = GetComponentInParent<ModulesObjectsBuffer>().Player.GetComponent<Animator>();
    }

    [Command]
    private void CmdChangeAnimatorLayerToHoldItem(int value)
    {
        RpcChangeAnimatorLayerToHoldItem(value);
    }

    [ClientRpc]
    private void RpcChangeAnimatorLayerToHoldItem(int value)
    {
        if(!_animator) _animator = GetComponentInParent<ModulesObjectsBuffer>().Player.GetComponent<Animator>();
        _animator.SetLayerWeight(_animator.GetLayerIndex(_axeHoldLayer), value);
        _animator.SetLayerWeight(_animator.GetLayerIndex(_itemHoldLayer), 0);
    }

    new private void Update()
    {
        base.Update();
    }

    [Server]
    public override void Use()
    {
        if (!_canAttack) return;

        _lastAttackTime = Time.time;
        RpcPlayAttackAnimation();
    }

    [ClientRpc]
    private void RpcPlayAttackAnimation()
    {
        _animator.SetTrigger("Attack");
    }
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {   if (!_isAttacking) return;
        if(other.gameObject.TryGetComponent(out IDamagable damagable) && other.gameObject.CompareTag("Survivor")) damagable.GetDamage(_damage);
    }
}
