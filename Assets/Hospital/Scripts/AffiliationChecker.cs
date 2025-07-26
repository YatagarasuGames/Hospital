using UnityEngine;
using Mirror;

public class AffiliationChecker : NetworkBehaviour, IAffiliated
{
    [field: SerializeField] public bool CheckAffiliation { get; private set; } = true;
    [field: SerializeField] public ItemAffiliation Affiliation { get; private set; }

    [Server]
    public bool HasAccess(uint playerNetId)
    {
        if (Affiliation == ItemAffiliation.All) return true;
        else return NetworkServer.spawned[playerNetId].CompareTag(Affiliation.ToString());
    }
}
