using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ChemistGasTrapPlacerModule : NetworkBehaviour
{
    [SerializeField] private GameObject _gasTrap;
    private Transform _camera;

    private void OnEnable()
    {
        _camera = transform.parent.GetComponent<ModulesObjectsBuffer>().Camera.transform;
    }
    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!isOwned || _gasTrap == null) return;

            if (isServer) PlaceTrap();
            else CmdPlaceTrap();
        }
    }

    [Server]
    private void PlaceTrap()
    {
        GameObject spawnedTrap = Instantiate(_gasTrap, _camera.transform.localPosition + transform.forward * 2, Quaternion.identity);
        NetworkServer.Spawn(spawnedTrap);

    }

    [Command]
    private void CmdPlaceTrap()
    {
        PlaceTrap();
    }
}
