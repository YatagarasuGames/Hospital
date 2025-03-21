using Mirror;
using UnityEngine;

public class TrapPlacer : NetworkBehaviour
{
    [SerializeField] private GameObject _trap; // ������ �������
    private Transform _camera;
    private string _trapPrefabName = "Trap"; // ��� ������� �������

    private void OnEnable()
    {
        _camera = transform.parent.GetComponentInChildren<Camera>().transform;
    }
    private void Update()
    {
        // ������ ��� ���������� ������ � ���� ������ ���������������


        // �� ������� ������� T ��������� �������
        if (Input.GetKeyDown(KeyCode.T))
        {
            print(isOwned);
            print(isLocalPlayer);
            if (!isOwned || _trap == null) return;

            if (isServer) PlaceTrap();
            else CmdPlaceTrap();
        }
    }

    [Server]
    private void PlaceTrap()
    {
        GameObject spawnedTrap = Instantiate(_trap, _camera.transform.localPosition + transform.forward * 2, Quaternion.identity);
        NetworkServer.Spawn(spawnedTrap); // �������������� � ���������

    }

    [Command]
    private void CmdPlaceTrap()
    {
        // ������� ������� �� �������
        PlaceTrap();
    }
}