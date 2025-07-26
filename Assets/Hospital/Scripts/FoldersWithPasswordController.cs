using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldersWithPasswordController : NetworkBehaviour
{
    [SerializeField] private List<PasswordTaskData> _passwordsData;
    [SerializeField] private List<PasswordFolder> _passwordFolders;
    [SerializeField] private EnterPasswordTaskGiver _enterPasswordTaskGiver;

    [SyncVar(hook = nameof(OnCorrectPasswordChanged))]
    private int _folderWithCorrectPasswordId;

    [SyncVar(hook = nameof(OnCorrectPasswordChanged))]
    private int _correctPasswordId;

    private void OnCorrectPasswordChanged(int _, int newValue)
    {
        UpdatePasswordOnClient();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        SetRandomPassword();
    }

    [Server]
    private void SetRandomPassword()
    {
        _folderWithCorrectPasswordId = Random.Range(0, _passwordFolders.Count);
        _correctPasswordId = Random.Range(0, _passwordsData.Count);
        _enterPasswordTaskGiver.SetPassword(_passwordsData[_correctPasswordId].Password);
    }

    private void UpdatePasswordOnClient()
    {
        if (!isClient) return;

        var folder = _passwordFolders[_folderWithCorrectPasswordId];
        folder.SetPasswordData(_passwordsData[_correctPasswordId]);
    }
}
