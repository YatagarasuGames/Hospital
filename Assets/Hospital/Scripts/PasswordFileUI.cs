using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordFileUI : UI
{
    [SerializeField] private Image _passwordDataImage;
    public override void Init(GameObject uiCreator)
    {
        base.Init(uiCreator);
        _passwordDataImage.sprite = _uiCreator.GetComponent<PasswordFolder>().PasswordData.PasswordSprite;
    }
}
