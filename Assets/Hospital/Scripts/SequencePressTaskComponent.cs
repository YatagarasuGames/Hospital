using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequencePressTaskComponent : NetworkBehaviour, IInteract
{
    [SerializeField][SyncVar] private int _pressOrder;
    [SerializeField] private SequencePressTaskController _controller;

    [Server]
    public void Init(int newPressOrderNumber) => _pressOrder = newPressOrderNumber;

    [Command]
    public void CmdInteract(uint interactCaller)
    {
        Interact(interactCaller);
    }

    [Server]
    public void Interact(uint interactCaller)
    {
        _controller.HandleComponentPressed(_pressOrder);
    }
}
