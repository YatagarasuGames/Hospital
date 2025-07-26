using Mirror;

public interface IDeadStateDependable
{
    [Server]
    public void ChangeState(bool newState);
}
