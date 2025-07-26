using Mirror;
using TMPro;
using UnityEngine;

public class MaxPlayersChanger : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentMaxPlayersText;

    public void SetNewMaxPlayers(float newMaxPlayers)
    {
        NetworkManager.singleton.maxConnections = (int)newMaxPlayers;
        _currentMaxPlayersText.text = newMaxPlayers.ToString();
    }
}
