using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerOutlineDetection : NetworkBehaviour
{
    [SerializeField] private GameObject _camera;
    private GameObject _outlinedObject;
    private OutlinableItem _outlinableObject;


    private void Update()
    {

        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.collider.gameObject.TryGetComponent(out OutlinableItem detectedObject))
            {
                if (detectedObject.enabled == false) return;
                _outlinedObject = hit.collider.gameObject;
                _outlinableObject = detectedObject;
                _outlinableObject.EnableOutline();
            }
            else
            {
                if (_outlinedObject == null) return;
                _outlinableObject.DisableOutline();
                _outlinedObject = null;
                _outlinableObject = null;
            }
        }
        else
        {
            if (_outlinedObject == null) return;
            _outlinableObject.DisableOutline();
            _outlinedObject = null;
            _outlinableObject = null;
        }
    }

}
