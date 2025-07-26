using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cysharp.Threading.Tasks;

public class AttachedTipster : NetworkBehaviour
{
    [SerializeField] private float _outlineDuration;
    [SerializeField] private float _detectionRadius = 4;

    public override void OnStopServer()
    {
        base.OnStopServer();
        NetworkConnectionToClient host = connectionToClient;
        List<GameObject> playersInDetectionZone = new List<GameObject>();
        foreach (var connection in NetworkServer.connections.Values)
        {
            if (Vector3.Distance(transform.position, connection.identity.transform.position) < _detectionRadius)
                playersInDetectionZone.Add(connection.identity.gameObject);
            if (connection.identity.CompareTag("Hunter"))
            {
                host = connection;
            }
        }

        if (playersInDetectionZone.Count == 0) return;
        foreach (var player in playersInDetectionZone)
        {
            player.GetComponent<NetworkOutline>().SetOutlineFormatForPlayer(host, true, _outlineDuration).Forget();
        }
    }
}
