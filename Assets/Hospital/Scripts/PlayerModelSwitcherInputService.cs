using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerModelSwitcherInputService : NetworkBehaviour
{
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private Transform _camera;
    private BaseClassSwitcher _currentClass;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if(isLocalPlayer) _playerModel.GetComponent<SkinnedMeshRenderer>().enabled = false;
    }


    private void Update()
    {
        if (isOwned)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (isServer) SwitchClass(netId);
                else CmdSwitchClass(netId);
            }
        }
    }

    [Server]
    private void SwitchClass(uint netId)
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, 2f))
        {
            if (hit.collider.gameObject.TryGetComponent(out BaseClassSwitcher baseClassSwitcher))
            {
                if (baseClassSwitcher == _currentClass) return;
                if(_currentClass != null) _currentClass.Remove(netId);
                _currentClass = baseClassSwitcher;
                _currentClass.Append(netId);
                print("Call sent");
            }
        }
    }

    [Command]
    private void CmdSwitchClass(uint netId)
    {
        SwitchClass(netId);
        print("CMD Call sent");
    }

}
