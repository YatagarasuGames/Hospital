using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerCollectService : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private GameObject _juice;
    [SerializeField] private GameObject _table;
    [SerializeField] private TMP_Text _juiceCountText;
    [SyncVar(hook = nameof(SyncJuiceCount))] private int _juiceCount;

    private void Update()
    {
        if (isOwned)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isServer) Collect();
                else CmdCollect();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isServer) CreateJuice();
                else CmdCreateJuice();
            }
        }
    }

    [Server]
    public void Collect()
    {
        RaycastHit hit;
        Debug.DrawRay(_camera.position, _camera.forward);
        if (Physics.Raycast(_camera.position, _camera.forward, out hit, 2f))
        {
            if(hit.collider.gameObject.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
                _juiceCount++;
                _juiceCountText.text = string.Format("JuiceCount: {0}", _juiceCount.ToString());
            }            
        }
    }

    public void SyncJuiceCount(int oldValue, int newValue)
    {
        _juiceCountText.text = string.Format("JuiceCount: {0}", newValue.ToString());
    }

    [Command]
    public void CmdCollect()
    {
        Collect();
    }

    [Server]
    public void CreateJuice()
    {
        var juice = Instantiate(_juice);
        NetworkServer.Spawn(juice);
        var table = Instantiate(_table);
        NetworkServer.Spawn(table);
    }

    [Command]
    public void CmdCreateJuice()
    {
        CreateJuice();
    }
}
