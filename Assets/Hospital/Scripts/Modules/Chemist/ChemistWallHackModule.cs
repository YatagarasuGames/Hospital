using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;

public class ChemistWallHackModule : ActiveModule
{
    private List<NetworkOutline> _players = new List<NetworkOutline>();
    [SerializeField] private float _wallhackDuration = 5f;

    public override void OnStartClient()
    {
        base.OnStartClient();
        GetAllPlayers();
    }

    private void GetAllPlayers()
    {
        _players.Clear();
        foreach (var tempObject in NetworkServer.spawned)
        {
            if (tempObject.Value.CompareTag("Survivor"))
            {
                print(tempObject.Value.gameObject.GetComponent<NetworkOutline>());
                _players.Add(tempObject.Value.gameObject.GetComponent<NetworkOutline>());
            }
        }
    }

    [Server]
    protected override void Use()
    {
        GetAllPlayers();
        print(_players.Count);
        foreach (NetworkOutline playerOutline in _players)
        {
            if(playerOutline == null) continue;
            print(playerOutline);
            NetworkConnectionToClient connection = GetComponent<NetworkIdentity>().connectionToClient;
            playerOutline.SetOutlineFormatForPlayer(connection, true, _wallhackDuration).Forget();
            //RpcSetOutlinedMaterial(playerOutline.GetComponent<NetworkIdentity>().netId, true);
        }
    }

    protected override void OnBufferEnabled(GameObject old, GameObject newBuffer) { } 
}
