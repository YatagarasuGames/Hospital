using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHKeyCard : InHandItem
{
    [SerializeField] private DoorKeyType _type;
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
            if (hit.collider.gameObject.TryGetComponent(out KeyCardReader door))
            {
                if (door.Type == _type)
                {
                    door.CheckCard();
                    onItemUsed?.Invoke(connectionToClient);
                }
            }
        }
    }
}
