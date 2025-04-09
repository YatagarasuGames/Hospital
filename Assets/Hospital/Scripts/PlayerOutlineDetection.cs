using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerOutlineDetection : NetworkBehaviour
{
    [SerializeField] private GameObject _camera;

    [SyncVar] private uint _outlinableObjectId = 0;


    private void Update()
    {

        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.collider.gameObject.TryGetComponent(out OutlinableItem detectedObject))
            {
                if (detectedObject.enabled == false) return;
                _outlinableObjectId = hit.collider.gameObject.GetComponent<NetworkIdentity>().netId;

                if (NetworkServer.spawned.TryGetValue(_outlinableObjectId, out NetworkIdentity outlinedObject))
                {
                    if (isServer) outlinedObject.GetComponent<OutlinableItem>().Outline.SetOutlineFormat(GetComponent<NetworkIdentity>().connectionToClient, true);
                    else outlinedObject.GetComponent<OutlinableItem>().Outline.CmdSetOutlineFormat(GetComponent<NetworkIdentity>().connectionToClient, true);
                    print("a");
                }
                
            }
            else
            {
                if (_outlinableObjectId == 0) return;

                if(NetworkServer.spawned.TryGetValue(_outlinableObjectId, out NetworkIdentity outlinedObject))
                {
                    if (isServer) outlinedObject.GetComponent<OutlinableItem>().Outline.SetOutlineFormat(GetComponent<NetworkIdentity>().connectionToClient, false);
                    else outlinedObject.GetComponent<OutlinableItem>().Outline.CmdSetOutlineFormat(GetComponent<NetworkIdentity>().connectionToClient, false);
                }
                    

                _outlinableObjectId = 0;
                print("b");
            }
        }
        else
        {
            if(_outlinableObjectId != 0)
            {
                if (NetworkServer.spawned.TryGetValue(_outlinableObjectId, out NetworkIdentity outlinedObject))
                {
                    if(isServer) outlinedObject.GetComponent<OutlinableItem>().Outline.SetOutlineFormat(GetComponent<NetworkIdentity>().connectionToClient, false);
                    else outlinedObject.GetComponent<OutlinableItem>().Outline.CmdSetOutlineFormat(GetComponent<NetworkIdentity>().connectionToClient, false);
                    print("c");
                }
            }
            _outlinableObjectId = 0;
        }
    }

}
