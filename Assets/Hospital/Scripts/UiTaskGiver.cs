using Mirror;
using UnityEngine;

[RequireComponent(typeof(AffiliationChecker))]
public class UiTaskGiver : LocalUICreator
{
    protected GameObject _currentTaskObject;

    [SyncVar] protected bool _isInteracting = false;
    [SyncVar] protected bool _isCompleted = false;

    [Server]
    public override void Interact(uint interactCaller)
    {
        if (_isCompleted || _isInteracting) return;

        base.Interact(interactCaller);
        _isInteracting = true;
    }

    [Command]
    public override void CmdInteract(uint interactCaller)
    {
        base.CmdInteract(interactCaller);
    }

    [Server]
    public virtual void HandleLeaveTask()
    {
        _isInteracting = false;
    }

    [Command(requiresAuthority = false)]
    public virtual void CmdHandleLeaveTask()
    {
        HandleLeaveTask();
    }

    [Command(requiresAuthority = false)]
    public virtual void CmdHandleTaskCompleted()
    {
        _isCompleted = true;
    }
}
