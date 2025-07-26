using Mirror;

interface ILongInteractable
{
    [Server]
    public void InteractStep(uint interactCaller);

    [Server]
    public void CompleteInteract();
}
