using Mirror;
public interface IInteract
{
    [Server]
    public void Interact(uint interactCaller);

    [Command]
    public void CmdInteract(uint interactCaller);
}
