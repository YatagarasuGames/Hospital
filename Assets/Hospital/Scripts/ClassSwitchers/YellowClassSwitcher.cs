using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowClassSwitcher : BaseClassSwitcher
{
    [Server]
    public override void Append(uint callerNetId)
    {
        if (NetworkServer.spawned.TryGetValue(callerNetId, out var instance))
        {
            //instance.gameObject.AddComponent(typeof(YellowClassSpeedOverride));
            print("Added");
        }
    }

    [Server]
    public override void Remove(uint callerNetId)
    {
        if (NetworkServer.spawned.TryGetValue(callerNetId, out var instance))
        {
            //Destroy(instance.gameObject.GetComponent<YellowClassSpeedOverride>());
        }
    }
}
