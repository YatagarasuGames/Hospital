using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerClass : NetworkBehaviour
{
    [SerializeField, SyncVar(hook = nameof(OnClassTypeChanged))] private ClassType _classType = ClassType.@default;

    [Server]
    public void SetClass(ClassType classType)
    {
        _classType = classType;
    }

    [Server]
    private void OnClassTypeChanged(ClassType oldType, ClassType newType)
    {
        print("GFJDGJHDFGJGUIEG");
        ClassesController.Instance.HandleClassSwitched(oldType, newType, GetComponent<NetworkIdentity>());
    }

    public override void OnStopServer()
    {
        ClassesController.Instance.HandlePlayerLeft(GetComponent<NetworkIdentity>());
        base.OnStopServer();
    }
}
