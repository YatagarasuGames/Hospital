using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlayerModelSwitcherInputService : NetworkBehaviour
{
    [SerializeField] private GameObject _playerModel;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if(isLocalPlayer) _playerModel.SetActive(false);
    }


}
