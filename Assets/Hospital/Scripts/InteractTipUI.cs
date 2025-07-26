using UnityEngine;

/// <summary>
/// UI 3D object 
/// </summary>
public class InteractTipUI : InteractTip
{
    public override void Init(Transform interacter)
    {
        base.Init(interacter);
        GetComponent<Canvas>().worldCamera = interacter.GetComponentInChildren<Camera>();
    }
    /// <summary>
    /// Update progress of UI visual with given params
    /// </summary>
    public virtual void UpdateProgress(float newProgress) { }
}
