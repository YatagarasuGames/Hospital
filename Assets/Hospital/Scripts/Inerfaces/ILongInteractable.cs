using Mirror;

interface ILongInteractable
{
    [Server]
    public void InteractStep();

    [Server]
    public void CompleteInteract();
}
