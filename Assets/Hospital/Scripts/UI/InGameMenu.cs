using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InGameMenu : UI
{
    [SerializeField] private UnityEvent _onContinueButtonPressed;
    [Space(10)]
    [SerializeField] private GameObject _settingsMenu;

    public void Continue()
    {
        LeaveTask();
    }
    public void Exit()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            Destroy(NetworkManager.singleton.gameObject);
            print("NetworkManager deleted");
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            // Клиент отключается
            Destroy(NetworkManager.singleton.gameObject);
            NetworkManager.singleton.StopClient();
        }
        
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        _settingsMenu.SetActive(false);
    }
}
