using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHJuice : InHandItem
{
    [SerializeField] private float _speedBoostValue;
    [SerializeField] private float _speedBoostDuration;

    public override void Update()
    {
        base.Update();
    }

    public override void Use()
    {
        GetComponentInParent<ModulesObjectsBuffer>().Player.GetComponent<PlayerSprint>().AddSprintTimeBuff(_speedBoostDuration, 2);
        onItemUsed?.Invoke(connectionToClient);
    }
}
