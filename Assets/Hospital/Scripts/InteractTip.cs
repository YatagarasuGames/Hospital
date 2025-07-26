using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Base class for UI objects in 3D space.
/// </summary>
public abstract class InteractTip : MonoBehaviour
{
    protected Transform _interacter;
    protected Transform _parent;
    [SerializeField] private Vector3 _offset;

    /// <summary>
    /// Type initialize logic here.
    /// </summary>
    /// <param name="interacter">Interacter player camera transform</param>
    public virtual void Init(Transform interacter)
    {
        _interacter = interacter;
        _parent = transform.parent;
    }

    /// <summary>
    /// Rotate UI object to interact player. Use in LateUpdate().
    /// </summary>
    protected virtual void RotateToPlayer()
    {
        transform.position = _parent.position + _offset;

        Vector3 directionToPlayer = _interacter.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer);
        transform.rotation = targetRotation;
        
    }
}
