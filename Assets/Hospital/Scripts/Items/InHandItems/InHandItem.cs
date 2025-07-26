using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InHandItem : NetworkBehaviour
{
    [field: SerializeField] public ItemInHandData Data { get; private set; }

    public static Action<NetworkConnectionToClient> onItemUsed;

    [SerializeField] protected GameObject _droppableItem;
    public GameObject DroppableItem => _droppableItem;

    public virtual void Update()
    {
        if (!isOwned) return;

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    if (isServer) Use();
        //    else CmdUse();
        //}
    }
    [Server]
    public virtual void Use() { }

    [Command]
    private void CmdUse() { Use(); }
}
