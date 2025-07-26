using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerOutlineDetection : NetworkBehaviour
{
    [SerializeField] private GameObject _camera;
    [SyncVar] private uint _outlinableObjectId = 0;

    [SyncVar] private bool _wasAffiliationChecked = false;
    [SyncVar] private bool _canDisplayUiBasedOnAffiliation = false;

    private void Update()
    {
        if(!isOwned) return;
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.collider.gameObject.TryGetComponent(out OutlinableItem detectedObject))
            {
                if (!_wasAffiliationChecked) CmdCheckAffiliation(hit.collider.gameObject.GetComponent<NetworkIdentity>(), NetworkClient.localPlayer.netId);
                
                _outlinableObjectId = detectedObject.GetComponent<NetworkIdentity>().netId;
                if (_canDisplayUiBasedOnAffiliation) detectedObject.Outline.SetOutlineFormatLocal(true);
                
            }
            else
            {
                if (_outlinableObjectId == 0) return;

                if(NetworkClient.spawned.TryGetValue(_outlinableObjectId, out NetworkIdentity outlinedObject))
                {
                    outlinedObject.GetComponent<OutlinableItem>().Outline.SetOutlineFormatLocal(false);
                    CmdResetAffiliatioCheck();
                }
                _outlinableObjectId = 0;
                print("b");
            }
        }

        else
        {
            if(_outlinableObjectId != 0)
            {
                if (NetworkClient.spawned.TryGetValue(_outlinableObjectId, out NetworkIdentity outlinedObject))
                {
                    outlinedObject.GetComponent<OutlinableItem>().Outline.SetOutlineFormatLocal(false);
                    print("c");
                }
                CmdResetAffiliatioCheck();
                _outlinableObjectId = 0;
            }
        }
    }

    [Command]
    private void CmdCheckAffiliation(NetworkIdentity observedObject, uint interacter)
    {
        if (observedObject.TryGetComponent(out IAffiliated affiliated)) _canDisplayUiBasedOnAffiliation = affiliated.HasAccess(interacter);
        else _canDisplayUiBasedOnAffiliation = true;
        _wasAffiliationChecked = true;
    }

    [Command]
    private void CmdResetAffiliatioCheck()
    {
        _wasAffiliationChecked = false;
        _canDisplayUiBasedOnAffiliation = false;
        print("done");
    }

}
