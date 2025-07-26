using Mirror;

public class ReadyUpStartGameButton : NetworkBehaviour, IInteract
{
    [Command]
    public void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }

    [Server]
    public void Interact(uint interactCaller)
    {
        TryStartGame(interactCaller);
    }

    [Server]
    private void TryStartGame(uint interactCaller)
    {
        ReadyUpController.Instance.TryStartGame(NetworkServer.spawned[interactCaller]);
    }
}
