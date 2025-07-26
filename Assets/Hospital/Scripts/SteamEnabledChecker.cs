using Steamworks;
using UnityEngine;

public class SteamEnabledChecker : MonoBehaviour
{
    [SerializeField] private GameObject _steamInitError;
    [SerializeField] private GameObject _gameMainMenu;
    private void OnEnable()
    {
        if (!SteamManager.Initialized)
        {
            _steamInitError.SetActive(true);
        }
        else
        {
            _gameMainMenu.SetActive(true);
        }
    }
}
