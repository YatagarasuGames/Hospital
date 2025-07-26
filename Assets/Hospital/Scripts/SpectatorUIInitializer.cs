using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpectatorUIInitializer : NetworkBehaviour
{
    [SerializeField] private TMP_Text _currentlySpectatablePlayerText;

    public void Init(uint targetNetId)
    {
        if(NetworkClient.spawned.TryGetValue(targetNetId, out NetworkIdentity targetPlayer))
        {
            _currentlySpectatablePlayerText.text = $"Currently spectating: {targetPlayer.GetComponent<PlayerNicknameLoader>().Nickname}";
            
        }
    }
}
