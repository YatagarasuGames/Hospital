using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseClassSwitcher : NetworkBehaviour
{
    [SerializeField] protected Mesh _classCharacterMesh;
    public abstract void Append(uint callerNetId);
    public abstract void Remove(uint callerNetId);
}
