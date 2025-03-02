using Mirror;
public interface ICollectable
{
    [Server]
    public void Collect();

    [Command]
    public void CmdCollect();
}
