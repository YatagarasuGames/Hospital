using Mirror;

public interface IAffiliated
{
    public bool CheckAffiliation { get; }
    public ItemAffiliation Affiliation { get; }

    public bool HasAccess(uint playerNetId);
}
