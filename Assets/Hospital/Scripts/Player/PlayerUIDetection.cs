using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIDetection : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private List<LayerMask> _layers;
    private RaycastHit _hit;
    public GameObject CurrentTip { get; private set; }

    private bool _isTipCreated = false;

    [SyncVar] private bool _wasAffiliationChecked = false;
    [SyncVar] private bool _canDisplayUiBasedOnAffiliation = false;

    private void FixedUpdate()
    {
        if(!isOwned) return;
        DetectUI();
    }

    private void DetectUI()
    {
        if (Physics.Raycast(_camera.position, _camera.forward, out _hit, 2f))
        {
            if (!CheckLayer(_hit.collider.gameObject.layer))
            {
                if (_isTipCreated) DestroyTip();
                return;
            }

            if (_hit.collider.TryGetComponent(out InteractTipGiver interactTip))
            {
                if (_isTipCreated) return;
                if (!_wasAffiliationChecked) CmdCheckAffiliation(_hit.collider.gameObject.GetComponent<NetworkIdentity>(), NetworkClient.localPlayer.netId);
                if (!_canDisplayUiBasedOnAffiliation) return;

                CurrentTip = Instantiate(interactTip.Tip, _hit.transform);
                CurrentTip.GetComponent<InteractTip>().Init(transform);
                _isTipCreated = true;
                return;
            }
            else if (_isTipCreated) DestroyTip();
        }
        else if (_isTipCreated) DestroyTip();

    }

    private bool CheckLayer(int hitIndexId)
    {
        foreach (var layer in _layers)
        {
            if (layer.value == Mathf.Pow(2, hitIndexId)) return true;
        }
        CmdResetAffiliatioCheck();
        return false;
    }

    [Command]
    private void CmdCheckAffiliation(NetworkIdentity observedObject, uint interacter)
    {
        if (observedObject.TryGetComponent(out IAffiliated affiliated)) _canDisplayUiBasedOnAffiliation = affiliated.HasAccess(interacter);
        else _canDisplayUiBasedOnAffiliation = true;
        _wasAffiliationChecked = true;
    }
    private void DestroyTip()
    {
        if(!CurrentTip) return;
        Destroy(CurrentTip);
        _isTipCreated = false;
        CmdResetAffiliatioCheck();
    }

    [Command]
    private void CmdResetAffiliatioCheck()
    {
        _wasAffiliationChecked = false;
        _canDisplayUiBasedOnAffiliation = false;
    }

    private void OnDisable()
    {
        if (_isTipCreated)
        {
            DestroyTip();
            CmdResetAffiliatioCheck();
        }
    }
}
