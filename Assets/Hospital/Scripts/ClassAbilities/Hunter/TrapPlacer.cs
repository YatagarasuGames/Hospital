using Mirror;
using UnityEngine;

public class TrapPlacer : NetworkBehaviour
{
    [SerializeField] private GameObject _trap; // Префаб ловушки
    private Transform _camera;
    private string _trapPrefabName = "Trap"; // Имя префаба ловушки

    private void OnEnable()
    {
        _camera = transform.parent.GetComponentInParent<ModulesObjectsBuffer>().Camera.transform;
    }
    private void Update()
    {   
        // Только для локального игрока и если префаб инициализирован


        // По нажатию клавиши T размещаем ловушку
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
        NetworkServer.Spawn(spawnedTrap); // Синхронизируем с клиентами

    }

    [Command]
    private void CmdPlaceTrap()
    {
        // Создаем ловушку на сервере
        PlaceTrap();
    }
}