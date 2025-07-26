using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.XR;

public class PlayerRagdollController : NetworkBehaviour
{
    [SyncVar(hook = nameof(EnableRagdoll))] private bool _isRagdollEnabled = false;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _ragdollRoot;

    private Rigidbody[] _rigidbodies;
    private CharacterJoint[] _characterJoints;
    private Collider[] _colliders;

    public override void OnStartClient()
    {
        base.OnStartAuthority();
        PlayerHealth.OnPlayerDied += HandlePlayerDied;
        CmdInit();
    }

    private void HandlePlayerDied(NetworkIdentity player)
    {
        if (player != GetComponent<NetworkIdentity>()) return;
        CmdEnableRagdollBool();
    }
    [Command]
    private void CmdEnableRagdollBool() => _isRagdollEnabled = true;

    private void EnableRagdoll(bool oldValue, bool newValue)
    {
        if (!isOwned) return;
        CmdEnableRagdoll();
    }
    [Command]
    private void CmdEnableRagdoll() => RpcEnableRagdoll();

    [ClientRpc]
    private void RpcEnableRagdoll()
    {
        _rigidbodies = _ragdollRoot.GetComponentsInChildren<Rigidbody>();
        _characterJoints = _ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        _colliders = _ragdollRoot.GetComponentsInChildren<Collider>();
        print(_animator);
        _animator.enabled = false;
        foreach (var rb in _rigidbodies) { print(rb); rb.isKinematic = false; }
        foreach (var joint in _characterJoints) { print(joint); joint.enableCollision = true; }
        foreach (var collider in _colliders) { print(collider); collider.enabled = true; }
        foreach (var controller in GetComponentsInChildren<IDeadStateDependable>()) { print(controller); controller.ChangeState(false); }
        foreach (var controller in GetComponents<IDeadStateDependable>()) { print(controller); controller.ChangeState(false); }
    }

    [Command]
    private void CmdInit()
    {
        _rigidbodies = _ragdollRoot.GetComponentsInChildren<Rigidbody>();
        _characterJoints = _ragdollRoot.GetComponentsInChildren<CharacterJoint>();
        _colliders = _ragdollRoot.GetComponentsInChildren<Collider>();
        _animator.enabled = true;
        foreach (var rb in _rigidbodies) rb.isKinematic = true;
        foreach (var joint in _characterJoints) joint.enableCollision = false;
        foreach (var collider in _colliders) collider.enabled = false;
        print("Cmd Init in ragdoll done");
    }

    public override void OnStopClient()
    {
        PlayerHealth.OnPlayerDied -= HandlePlayerDied;
    }
}
