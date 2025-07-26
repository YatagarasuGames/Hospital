using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuInput : InputComponent
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private PlayerUIDrawer _playerUIDrawer;

    private void Update()
    {
        if(!isOwned || !_isActive) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnableMenu();
        }
    }

    private void EnableMenu()
    {
        _playerUIDrawer.CreateUI(_menu, _playerUIDrawer.gameObject);
    }
}
