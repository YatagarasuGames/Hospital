using Mirror;
using UnityEngine;

public class TrapPlacer : NetworkBehaviour
{
    [SerializeField] private GameObject _trap;
    private Transform _camera;

    private void OnEnable()
    {
        _camera = transform.parent.GetComponent<ModulesObjectsBuffer>().Camera.transform;
    }
    private void Update()
    {   


        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!isOwned || _trap == null) return;

            if (isServer) PlaceTrap();
            else CmdPlaceTrap();
        }
    }

    [Server]
    private void PlaceTrap()
    {
        GameObject spawnedTrap = Instantiate(_trap, _camera.transform.localPosition + transform.forward * 2, Quaternion.identity);
        NetworkServer.Spawn(spawnedTrap); 

    }

    [Command]
    private void CmdPlaceTrap()
    {
        PlaceTrap();
    }
}