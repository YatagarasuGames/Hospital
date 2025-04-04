using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
    private readonly float _baseSpeed = 5f;
    [SerializeField, SyncVar] private float _speed = 5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SyncVar] private bool isTrapped = false;

    public static Action<float, float> onSpeedBuff;
    public static Action<float, float> onSpeedDebuff;

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (_rb.isKinematic) return;

        _animator.SetBool("Walking", _rb.velocity.magnitude >= 0.2f);

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

    [Server]
    public void SetTrappedState(bool newTrappedState, float duration)
    {
        isTrapped = newTrappedState;
        _rb.isKinematic = isTrapped;
        RpcUpdateTrappedState(isTrapped);
        StartCoroutine(UntrapPlayer(duration));
    }

    [Server]
    private IEnumerator UntrapPlayer(float duration)
    {
        yield return new WaitForSeconds(duration);
        SetTrappedState(false);
    }

    [ClientRpc]
    private void RpcUpdateTrappedState(bool trapped)
    {
        _rb.isKinematic = trapped;
    }



    [Server]
    public void AddSpeedBuff(float speedBuff, bool invokeSpeedboostEvent = false, float duration = 0f)
    {
        if(speedBuff <= 0) return;
        _speed += speedBuff;
        if (invokeSpeedboostEvent) onSpeedBuff?.Invoke(speedBuff, duration);
        if(duration > 0f) StartCoroutine(RemoveSpeedBuff(duration, speedBuff));
    }

    [Server]
    public void AddSpeedDebuff(float speedDebuff, bool invokeSpeedboostEvent = false, float duration = 0f)
    {
        if(speedDebuff <= 0 || speedDebuff >= _speed) return;
        _speed -= speedDebuff;
        if(invokeSpeedboostEvent) onSpeedDebuff?.Invoke(speedDebuff, duration);
        if (duration > 0f) StartCoroutine(RemoveSpeedDebuff(duration, speedDebuff));
    }


    [Server]
    private IEnumerator RemoveSpeedBuff(float duration, float buff)
    {
        yield return new WaitForSeconds(duration);
        if (enabled) AddSpeedDebuff(buff);
    }

    [Server]
    private IEnumerator RemoveSpeedDebuff(float duration, float debuff)
    {
        yield return new WaitForSeconds(duration);
        if (enabled) AddSpeedBuff(debuff);
    }
}