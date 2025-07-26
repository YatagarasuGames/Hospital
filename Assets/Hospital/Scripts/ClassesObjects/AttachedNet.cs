using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AttachedNet : NetworkBehaviour, ILongInteractable
{
    //[SyncVar] private bool isActivated = false;
    [SerializeField] private float deactivationTime = 5f;
    private float deactivationDuration;
    private PlayerMovement _playerMovement;

    [Server]
    public void Init(uint playerId)
    {
        if(NetworkServer.spawned.TryGetValue(playerId, out NetworkIdentity player))
        {
            _playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    [Server]
    public void InteractStep(uint interactCaller)
    {
        deactivationDuration += Time.deltaTime;
        print(deactivationDuration);
        if (deactivationDuration >= deactivationTime) CompleteInteract();
    }

    [Server]
    public void CompleteInteract()
    {
        //isActivated = false;

        _playerMovement.SetTrappedState(false);

        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
        
    }
}
