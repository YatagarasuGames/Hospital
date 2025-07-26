using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesHintInput : InputComponent
{
    [SerializeField] private GameObject _hintMenu;
    [SerializeField] private PlayerUIDrawer _playerUIDrawer;

    private void Update()
    {
        if (!isOwned || !_isActive) return;

        if (Input.GetKeyDown(KeyCode.F1))
        {
            EnableHint();
        }
    }

    private void EnableHint()
    {
        _playerUIDrawer.CreateUI(_hintMenu, _playerUIDrawer.gameObject);
    }
}
