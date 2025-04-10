using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnMenuStateChanged))]
    private bool _isMenuOpen;

    [SerializeField] private GameObject menuPanel;

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.Escape)) CmdToggleMenu();
    }

    [Command]
    private void CmdToggleMenu()
    {
        print("Clciked");
        _isMenuOpen = !_isMenuOpen;
    }
    private void OnMenuStateChanged(bool oldValue, bool newValue)
    {
        menuPanel.SetActive(newValue);
        Cursor.lockState = newValue ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    public void Continue() => CmdToggleMenu();
    public void Exit()
    {
        
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            // ���� ������������� � ������, � ������
            Destroy(NetworkManager.singleton.gameObject);
            print("NetworkManager deleted");
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            // ������ �����������
            Destroy(NetworkManager.singleton.gameObject);
            NetworkManager.singleton.StopClient();
        }
        else if (NetworkServer.active)
        {
            // ���������� ������
            Destroy(NetworkManager.singleton.gameObject);
            NetworkManager.singleton.StopServer();
        }
        
        // ������������ � ������� ���� (������)
        
        SceneManager.LoadScene("Menu");
    }
}
