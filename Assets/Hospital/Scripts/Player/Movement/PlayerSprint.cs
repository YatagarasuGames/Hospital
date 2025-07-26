using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;

public class PlayerSprint : NetworkBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField, Range(0, 5f)]private float sprintSpeedAdd = 1.5f;
    [SerializeField, SyncVar] private float sprintOverallTime = 4;
    [SyncVar] private float sprintTime;

    [SyncVar(hook = nameof(ChangeSpeedBuff))] private bool isSprinting = false;

    [SyncVar] private bool readyToSprint = true;

    private void Awake()
    {
        sprintTime = sprintOverallTime;
    }

    private void Update()
    {
        if(!isOwned) return;
        Sprint();
    }
    public void Sprint()
    {
        HandleSprintInput();
        CmdHandleStaminaChanged();
    }

    private void HandleSprintInput()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && readyToSprint) CmdSetSprint(true);

        if (Input.GetKeyUp(KeyCode.LeftShift)) CmdSetSprint(false);
    }

    [Command]
    private void CmdHandleStaminaChanged()
    {
        if (isSprinting)
        {
            UpdateStamina(-Time.deltaTime);
            if (sprintTime <= 0.3f)
            {
                readyToSprint = false;
                SetSprint(false);
                
            }
        }

        else
        {
            UpdateStamina(Time.deltaTime);
            if (sprintTime >= sprintOverallTime * 0.75f) readyToSprint = true;
        }
    }

    [Server]
    private void SetSprint(bool state) => isSprinting = state;

    [Command]
    private void CmdSetSprint(bool state) => isSprinting = state;

    [Server]
    private void UpdateStamina(float delta)
    {
        sprintTime = Mathf.Clamp(sprintTime + delta, 0f, sprintOverallTime);
    }

    private void ChangeSpeedBuff(bool oldValue, bool newValue)
    {
        if (!isServer) return;
        if (newValue) _playerMovement.AddSpeedBuff(sprintSpeedAdd);
        else _playerMovement.AddSpeedDebuff(sprintSpeedAdd);
    }

    #region Buffs
    /// <summary>
    /// <para>Add sprintTime buff(player can run longer)</para>
    /// <para>In case if duration = 0 it will give this buff for all game duration, use this only for modules or very carefully</para>
    /// <para>currentSprintTimeMultiplier - for how much you want to increase base sprintTime</para>
    /// </summary>
    [Server]
    public void AddSprintTimeBuff(float duration, float currentSprintTimeMultiplier)
    {
        sprintOverallTime = sprintOverallTime * currentSprintTimeMultiplier;
        sprintTime = sprintOverallTime;
        if (duration != 0) RemoveSprintTimeBuff(duration, currentSprintTimeMultiplier).Forget();
    }

    [Server]
    public async UniTaskVoid RemoveSprintTimeBuff(float delay, float multiplier)
    {
        await UniTask.Delay((int)delay*1000);
        sprintOverallTime /= multiplier;
        if (sprintTime > sprintOverallTime) sprintTime = sprintOverallTime;
    }

    /// <summary>
    /// <para>Add sprintSpeed buff(player sprint speed)</para>
    /// <para>In case if duration = 0 it will give this buff for all game duration, use this only for modules or very carefully</para>
    /// <para>currentSprintTimeMultiplier - for how much you want to increase current sprintSpeed</para>
    /// </summary>
    [Server]
    public void AddSprintSpeedBuff(float duration, float currentSprintSpeedMultiplier)
    {
        if (isSprinting)
        {
            SetSprint(false);
            sprintSpeedAdd = sprintSpeedAdd * currentSprintSpeedMultiplier;
            SetSprint(true);
        }
        else
        {
            sprintSpeedAdd = sprintSpeedAdd * currentSprintSpeedMultiplier;
        }
        if (duration != 0) RemoveSprintSpeedBuff(duration, currentSprintSpeedMultiplier).Forget();
    }

    [Server]
    private async UniTaskVoid RemoveSprintSpeedBuff(float delay, float multiplier)
    {
        await UniTask.Delay((int)delay * 1000);

        if (isSprinting)
        {
            SetSprint(false);
            sprintSpeedAdd /= multiplier;
            SetSprint(true);
        }
        else
        {
            sprintSpeedAdd /= multiplier;
        }

        
    }
    #endregion
}
