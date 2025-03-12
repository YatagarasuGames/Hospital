public class PlayerTrappedState
{
    public bool IsTrapped { get; private set; } = false;

    public void TrapPlayer()
    {
        if (IsTrapped) return;
        IsTrapped = true;
    }
    public void UnTrapPlayer()
    {
        if (!IsTrapped) return;
        IsTrapped = false;
    }
}
