using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHCrowbar : InHandItem
{
    [SerializeField] private float _useDistance;
    private Transform _camera;
    new private void Update()
    {
        base.Update();
    }

    public override void Use()
    {
        if (_camera == null) _camera = GetComponentInParent<ModulesObjectsBuffer>().Camera.transform;
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, _useDistance))
        {
            if(hit.collider.gameObject.TryGetComponent(out DoorDesk doorDesk))
            {
                doorDesk.Deattach();
            }
        }
    }
}
