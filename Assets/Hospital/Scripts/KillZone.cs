using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : NetworkBehaviour
{
    [Server]
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("Survivor"))

        other.gameObject.GetComponent<IDamagable>().GetDamage(50);
    }
}
