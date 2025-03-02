using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputServices : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    private PlayerCollectService _playerCollectService;
    private void Start()
    {
        //_playerCollectService = new PlayerCollectService(_camera);
    }
    private void Update()
    {
        if (isOwned)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _playerCollectService.Collect();
            }
        }
    }
}
