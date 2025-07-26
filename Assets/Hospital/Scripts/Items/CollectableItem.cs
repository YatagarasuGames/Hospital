using UnityEngine;
using Mirror;

[RequireComponent(typeof(AffiliationChecker))]
public class CollectableItem : NetworkBehaviour
{
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public GameObject InHandPrefab { get; private set; }
    public string Tip { get; set; } = "���\n����������� �������� ��������";

    [Server]
    public void Collect()
    {
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }
}
