using UnityEngine;

/// <summary>
/// Component which contain 3D GameObject of UI that must be used in 3D world space.
/// </summary>
public class InteractTipGiver : MonoBehaviour
{
    [field: SerializeField] public GameObject Tip { get; private set; }

}
