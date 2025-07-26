using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Collections;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : InputComponent
{
    [SerializeField, SyncVar] private float _speed = 5f;
    private float _baseSpeed = 5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rb;
    [SyncVar] private bool isTrapped = false;


    public static Action<float, float> onSpeedBuff;
    public static Action<float, float> onSpeedDebuff;

    public override void OnStartClient()
    {
        base.OnStartClient();
        _baseSpeed = _speed;
        //if(isLocalPlayer) foreach (var meshRenderer in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>()) meshRenderer.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        if (!isOwned || !_isActive) return;

        HandleInputAndAnimation();

        if (isClient)
        {
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (input != Vector2.zero) CmdSendMovementInput(input);
        }
    }

    [Command]
    private void CmdSendMovementInput(Vector2 input)
    {
        ServerMove(input);
    }

    [Server]
    private void ServerMove(Vector2 input)
    {
        Vector3 movementDirection = new Vector3(input.x * _speed, input.y * _speed);
        Vector3 movementDirection3 = transform.rotation * new Vector3(movementDirection.x, _rb.velocity.y, movementDirection.y);

        if (movementDirection3.magnitude > _speed)
            movementDirection3 = movementDirection3.normalized * _speed;

        _rb.velocity = movementDirection3;

        RpcSyncMovement(_rb.velocity);
    }

    [ClientRpc]
    private void RpcSyncMovement(Vector3 velocity)
    {
        //if (!isOwned) _rb.velocity = velocity;
        //else SmoothCorrectVelocity(velocity);
        if (isOwned) SmoothCorrectVelocity(velocity);
    }

    private IEnumerator SmoothCorrectVelocity(Vector3 targetVelocity)
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 startVelocity = _rb.velocity;

        while (elapsed < duration)
        {
            _rb.velocity = Vector3.Lerp(startVelocity, targetVelocity, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rb.velocity = targetVelocity;
    }

    private void HandleInputAndAnimation()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        _animator.SetBool("Walking", input.magnitude >= 0.2f);
        _animator.SetBool("Sprinting", _speed > _baseSpeed && input.magnitude >= 0.2f);
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


    /// <summary>
    /// <para>Add speed BUFF(>0) with given duration.</para>
    /// <para>In case if duration = 0 it will give this boost for all game duration, use this only for modules or very carefully</para>
    /// <para>InvokeSpeedboostEvent param must always be false. But if you want to let subscribers know that player got speed boost, make it true</para>
    /// </summary>
    [Server]
    public void AddSpeedBuff(float speedBuff, bool invokeSpeedboostEvent = false, float duration = 0f)
    {
        if(speedBuff <= 0) return;
        _speed += speedBuff;
        if (invokeSpeedboostEvent) onSpeedBuff?.Invoke(speedBuff, duration);
        if(duration > 0f) RemoveSpeedBuffAsync(duration, speedBuff).Forget();
    }

    /// <summary>
    /// <para>Add speed DEBUFF(<0) with given duration.</para>
    /// <para>In case if duration = 0 it will give this debuff for all game duration, use this only for modules or very carefully</para>
    /// <para>InvokeSpeedboostEvent param must always be false. But if you want to let subscribers know that player got speed debuff, make it true</para>
    /// </summary>
    [Server]
    public void AddSpeedDebuff(float speedDebuff, bool invokeSpeedboostEvent = false, float duration = 0f)
    {
        if(speedDebuff <= 0 || speedDebuff >= _speed) return;
        _speed -= speedDebuff;
        if (invokeSpeedboostEvent) onSpeedDebuff?.Invoke(speedDebuff, duration);
        if (duration > 0f) RemoveSpeedDebuffAsync(duration, speedDebuff).Forget();
    }

    [Server]
    private async UniTaskVoid RemoveSpeedBuffAsync(float duration, float buff)
    {
        await UniTask.Delay((int)duration * 1000);
        if (this == null || !this) return;
        AddSpeedDebuff(buff);
    }

    [Server]
    private async UniTaskVoid RemoveSpeedDebuffAsync(float duration, float debuff)
    {
        await UniTask.Delay((int)duration * 1000);
        if (this == null || !this) return;
        AddSpeedBuff(debuff);
    }

    protected override void OnModuleEnabled()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    protected override void OnModuleDisabled()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        _animator.SetBool("Walking", false);
        _animator.SetBool("Sprinting", false);
        CmdSendMovementInput(Vector3.zero);
    }

    private void OnDisable()
    {
        OnModuleDisabled();
    }
}