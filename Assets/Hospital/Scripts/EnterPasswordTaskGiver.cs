using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnterPasswordTaskGiver : UiTaskGiver
{
    [SyncVar(hook = nameof(HandlePasswordChanged))] private string _correctPassword;
    public string Password => _correctPassword;

    [Server]
    public void SetPassword(string password)
    {
        _correctPassword = password;
    }
    private void HandlePasswordChanged(string oldPassword, string newPassword)
    {
        _correctPassword = newPassword;
    }
}
