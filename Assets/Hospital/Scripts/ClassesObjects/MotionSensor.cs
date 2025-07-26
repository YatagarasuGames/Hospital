using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;

public class MotionSensor : NetworkBehaviour
{
    [SyncVar] private bool _isEnabled = true;
    [SerializeField] private float outlineTime;

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if(!_isEnabled) return;
        if (other.gameObject.CompareTag("Hunter"))
        {
            _isEnabled = false;
            other.gameObject.GetComponent<NetworkOutline>().SetOutlineFormatForAllPlayers(true, outlineTime).Forget();

        }
    }
}
