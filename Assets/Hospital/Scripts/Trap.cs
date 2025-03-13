using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Trap : NetworkBehaviour, ILongInteractable
{
    [SyncVar] private bool isActivated = false;
    [SerializeField] private float deactivationTime = 5f;
    private float deactivationDuration;
    private PlayerMovement player;

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;
        if (other.gameObject.CompareTag("Survivor"))
        {
            print("fdgfdgdg");
            isActivated = true;
            player = other.gameObject.GetComponent<PlayerMovement>();
            player.TrapPlayer();
            
        }
    }

    [Server]
    public void InteractStep()
    {
        if (!isActivated) { print("NotActivated yet"); return; }
        deactivationDuration += Time.deltaTime;
        print(deactivationDuration);
        if(deactivationDuration >= deactivationTime) CompleteInteract();
    }

    [Server]
    public void CompleteInteract()
    {
        isActivated = false;

        player.UnTrapPlayer();

        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }

}
