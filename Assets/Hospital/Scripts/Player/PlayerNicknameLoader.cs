using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNicknameLoader : NetworkBehaviour, IDeadStateDependable
{
    [SerializeField] private TMP_Text _nicknameText;
    [SyncVar(hook = nameof(OnNicknameSetted))] private string _nickname;
    public string Nickname => _nickname;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer) CmdChangeNickname(PlayerPrefs.GetString("Nickname"));
    }

    [Command]
    private void CmdChangeNickname(string newNick)
    {
        _nickname = newNick;
    }
    private void OnNicknameSetted(string oldNick, string newNick)
    {
        _nickname = newNick;
        _nicknameText.text = _nickname;
    }

    void IDeadStateDependable.ChangeState(bool newState)
    {
        CmdChangeState(newState);
    }
    [Command]
    private void CmdChangeState(bool newState) => _nicknameText.enabled = newState;
}
