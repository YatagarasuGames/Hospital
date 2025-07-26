using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHSyrengie : InHandItem
{
    [SerializeField] private float _buffDuration;
    [SerializeField] private float _buffMultiplier;

    [Server]
    public override void Use()
    {
        base.Use();
        GetComponentInParent<ModulesObjectsBuffer>().Player.GetComponent<PlayerSprint>().AddSprintSpeedBuff(_buffDuration, _buffMultiplier);
        onItemUsed?.Invoke(connectionToClient);

    }
}
