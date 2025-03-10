using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Trap : NetworkBehaviour, ILongInteractable
{
    private bool isActivated = false;
    [SerializeField] private float deactivationTime = 5f;
    private float deactivationDuration;
    private Rigidbody player;

    private void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;
        if (other.gameObject.CompareTag("Survivor"))
        {
            print("Player entered trap");
            isActivated = true;
            player = other.gameObject.GetComponent<Rigidbody>();
            print(player);
            NetworkIdentity trappedPlayer = player.GetComponent<NetworkIdentity>();
            Debug.LogWarning(trappedPlayer == null);
            if (isServer) { print("Server Call"); player.isKinematic = true; }
            else { print("Rpc call"); TargetTrapPlayer(trappedPlayer.connectionToClient); }
        }
    }

    [TargetRpc]
    private void TargetTrapPlayer(NetworkConnectionToClient networkConnection)
    {
        print("TargetCall");
        GetComponent<Rigidbody>().isKinematic = true;
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

        NetworkIdentity trappedPlayer = player.GetComponent<NetworkIdentity>();
        RpcUntrapPlayer(trappedPlayer.netId);

        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }

    [ClientRpc]
    public void RpcUntrapPlayer(uint netId)
    {
        if (this.netId == netId)
        {
            print(netId.ToString());
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

}
