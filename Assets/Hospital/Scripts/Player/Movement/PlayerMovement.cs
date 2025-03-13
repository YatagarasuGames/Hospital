using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SyncVar] public bool isTrapped = false;
    public PlayerTrappedState PlayerTrapped {  get; private set; }

    public readonly SyncList<float> speedOverrides = new SyncList<float>();

    public override void OnStartClient()
    {
        base.OnStartClient();
        PlayerTrapped = new PlayerTrappedState(_rb);
    }
    private void Move()
    {
        if (_rb.isKinematic || isTrapped) return;

        _animator.SetBool("Walking", _rb.velocity.magnitude >= 0.2f);
        if (speedOverrides.Count > 0)
        {
            _speed = speedOverrides[speedOverrides.Count - 1];
        }
        Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal") * _speed, Input.GetAxis("Vertical") * _speed);
        Vector3 movementDirection3 = transform.rotation * new Vector3(movementDirection.x, _rb.velocity.y, movementDirection.y);
        if (movementDirection3.magnitude > _speed) movementDirection3 = movementDirection3.normalized * _speed;
        _rb.velocity = movementDirection3;


    }

    private void FixedUpdate()
    {
        Move();
    }

    [Server]
    public void TrapPlayer()
    {
        isTrapped = true;
        RpcUpdateTrappedState(true);
    }

    [Server]
    public void UnTrapPlayer()
    {
        isTrapped = false;
        RpcUpdateTrappedState(false);
    }

    [ClientRpc] // Вызывается сервером, но выполняется на всех клиентах
    private void RpcUpdateTrappedState(bool trapped)
    {
        isTrapped = trapped;
        Debug.Log("tretertq");
    }

}