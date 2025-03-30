using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
    private readonly float _baseSpeed = 5f;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SyncVar] private bool isTrapped = false;

    public readonly SyncList<float> speedOverrides = new SyncList<float>();

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (_rb.isKinematic) return;

        _animator.SetBool("Walking", _rb.velocity.magnitude >= 0.2f);

        if (speedOverrides.Count > 0) _speed = speedOverrides[speedOverrides.Count - 1];
        else _speed = _baseSpeed;

            Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal") * _speed, Input.GetAxis("Vertical") * _speed);
        Vector3 movementDirection3 = transform.rotation * new Vector3(movementDirection.x, _rb.velocity.y, movementDirection.y);
        if (movementDirection3.magnitude > _speed) movementDirection3 = movementDirection3.normalized * _speed;
        _rb.velocity = movementDirection3;


    }



    [Server]
    public void SetTrappedState(bool newTrappedState)
    {
        isTrapped = newTrappedState;
        _rb.isKinematic = isTrapped;
        RpcUpdateTrappedState(isTrapped);
    }

    [ClientRpc]
    private void RpcUpdateTrappedState(bool trapped)
    {
        _rb.isKinematic = trapped;
    }

}