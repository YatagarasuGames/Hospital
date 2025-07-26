using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PricelController : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Image _pricel;
    [SerializeField] private Color _pricelInactive;
    [SerializeField] private Sprite _pricelInactiveSprite;
    [SerializeField] private Color _pricelActive;
    [SerializeField] private Sprite _pricelActiveSprite;
    private RaycastHit _hit;
    [SerializeField] private float _interactDistance;

    [SerializeField] private List<LayerMask> _layers;

    [SyncVar] private bool _wasAffiliationChecked = false;
    [SyncVar] private bool _canDisplayUiBasedOnAffiliation = false;
    private void Update()
    {
        if(!isOwned) return;
        if (Physics.Raycast(_camera.position, _camera.forward, out _hit, _interactDistance))
        {
            if (CheckLayer(_hit.collider.gameObject.layer))
            {

                if (!_wasAffiliationChecked)
                {
                    if (_hit.collider.gameObject.TryGetComponent(out NetworkIdentity identity)) CmdCheckAffiliation(identity.gameObject, NetworkClient.localPlayer.netId);
                    else CmdCheckAffiliationInChild(_hit.collider.gameObject.GetComponentInParent<NetworkIdentity>().gameObject, _hit.collider.gameObject.name, NetworkClient.localPlayer.netId);
                }
                if(_canDisplayUiBasedOnAffiliation) ActivatePricel();
            }
            else DeactivatePricel();
        }
        else DeactivatePricel();
    }

    private bool CheckLayer(int hitIndexId)
    {
        foreach (var layer in _layers)
        {
            if (layer.value == Mathf.Pow(2, hitIndexId)) return true;   
        }

        return false;
    }

    [Command]
    private void CmdCheckAffiliation(GameObject observedObject, uint interacter)
    {
        if (observedObject.TryGetComponent(out IAffiliated affiliated)) _canDisplayUiBasedOnAffiliation = affiliated.HasAccess(interacter);
        else _canDisplayUiBasedOnAffiliation = true;
        _wasAffiliationChecked = true;
    }

    [Command]
    private void CmdCheckAffiliationInChild(GameObject parent, string childName, uint interacter)
    {
        GameObject observedObject = parent.transform.Find(childName).gameObject;
        if (observedObject == null) Debug.LogError("Child object is NULL");
        if (observedObject.TryGetComponent(out IAffiliated affiliated)) _canDisplayUiBasedOnAffiliation = affiliated.HasAccess(interacter);
        else _canDisplayUiBasedOnAffiliation = true;
        _wasAffiliationChecked = true;
    }

    private void ActivatePricel()
    {
        _pricel.color = _pricelActive;
        _pricel.sprite = _pricelActiveSprite;
    }

    private void DeactivatePricel()
    {
        _pricel.color = _pricelInactive;
        _pricel.sprite = _pricelInactiveSprite;
        CmdResetAffiliatioCheck();
    }

    [Command]
    private void CmdResetAffiliatioCheck()
    {
        _wasAffiliationChecked = false;
        _canDisplayUiBasedOnAffiliation = false;
    }
}
