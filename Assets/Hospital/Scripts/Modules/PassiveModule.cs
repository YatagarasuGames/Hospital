using Mirror;

/// <summary>
/// Base class for passive modules like speed boost for athlete class
/// Call apply bonus method in Start() but you can override it and call this method whereever you want by using CmdApplyBonus() or ApplyBonus()
/// </summary>
public abstract class PassiveModule : Module
{
    protected NetworkIdentity _playerNetworkIdentity;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        OnModuleCreated?.Invoke(NetworkClient.localPlayer.netId, ModuleData, string.Empty);
    }

    [Command]
    public virtual void CmdApplyBonus()
    {
        ApplyBonus();
    }

    [Command]
    public virtual void CmdRemoveBonus()
    {
        RemoveBonus();
    }

    /// <summary>
    /// Add logic of applying bonus here
    /// </summary>
    protected abstract void ApplyBonus();

    /// <summary>
    /// Add logic of removing bonus effect here
    /// </summary>
    protected abstract void RemoveBonus();

    public override void OnStopServer()
    {
        RemoveBonus();
        base.OnStopServer();
    }
}
