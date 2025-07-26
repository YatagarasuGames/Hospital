using Mirror;
using UnityEngine;

public class IHTipster : InHandItem
{
    [SerializeField] private GameObject _attachedTipster;
    [SerializeField] private float _useDistance;
    private Transform _camera;
    [Server]
    public override void Use()
    {
        if (_camera == null) _camera = GetComponentInParent<ModulesObjectsBuffer>().Camera.transform;
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, _useDistance))
        {
            if (hit.collider.gameObject.TryGetComponent(out CollectableItem collectable))
            {
                if (collectable.GetComponent<AffiliationChecker>().Affiliation == ItemAffiliation.Survivor)
                {
                    var tempTipster = Instantiate(_attachedTipster, collectable.transform);
                    tempTipster.transform.position = collectable.transform.position;
                    NetworkServer.Spawn(tempTipster);
                    onItemUsed?.Invoke(connectionToClient);
                }
            }
        }
    }
}
