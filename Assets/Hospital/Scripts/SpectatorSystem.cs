using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class SpectatorSystem : NetworkBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private SpectatorUIInitializer _spectatorUIInitializer;
    private readonly SyncList<uint> _spectatableTargets = new SyncList<uint>();


    [SerializeField, Range(0, 2)] private float switchCooldown = 1f;

    [SyncVar] private int currentTargetIndex = -1;
    [SyncVar] private float lastSwitchTime;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        CmdSetActiveState();
        CmdFindSpectatableTargets();
        CmdChangeSpectatableTarget();
    }

    [Command]
    private void CmdSetActiveState()
    {
        RpcSetActiveState();
    }

    [ClientRpc]
    private void RpcSetActiveState()
    {
        gameObject.SetActive(isOwned);
    }

    private void Update()
    {
        if (!isOwned) return;
        if (Input.GetKeyDown(KeyCode.Tab) && Time.time - lastSwitchTime > switchCooldown)
        {
            CmdChangeSpectatableTarget();
            lastSwitchTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeMenuState();
        }
    }


    [Server]
    private void FindSpectatableTargets()
    {
        var temp = GameObject.FindGameObjectsWithTag("Survivor");
        foreach(var target in temp)
        {
            if (target != gameObject && target.activeSelf)
                _spectatableTargets.Add(target.GetComponent<NetworkIdentity>().netId);
        }
    }

    [Command]
    private void CmdFindSpectatableTargets()
    {
        FindSpectatableTargets();
       
    }

    [Server]
    private void ChangeSpectatableTarget()
    {
        if (_spectatableTargets == null || _spectatableTargets.Count == 0)
        {
            FindSpectatableTargets();
            if (_spectatableTargets.Count == 0) return;
        }

        currentTargetIndex = (currentTargetIndex + 1) % _spectatableTargets.Count;
        print(_spectatableTargets[currentTargetIndex]);
        AttachToTarget(_spectatableTargets[currentTargetIndex]);
    }

    [Command]
    private void CmdChangeSpectatableTarget()
    {
        ChangeSpectatableTarget();
    }

    [Server]
    private void AttachToTarget(uint targetNetId)
    {
        NetworkIdentity target;
        if(NetworkServer.spawned.TryGetValue(targetNetId, out target))
        {
            Transform targetCamera = target.GetComponentInChildren<Camera>().transform;
            transform.SetParent(targetCamera);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            TargetAttachToTarget(connectionToClient, targetNetId);
        }
    }

    public void ChangeMenuState()
    {
        bool isActive = !_menuPanel.activeInHierarchy;
        _menuPanel.SetActive(isActive);
        Cursor.lockState = isActive ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    [TargetRpc]
    private void TargetAttachToTarget(NetworkConnectionToClient networkConnectionToClient, uint targetNetId)
    {
        NetworkIdentity target;
        print(targetNetId);

        if (NetworkClient.spawned.TryGetValue(targetNetId, out target))
        {
            Transform targetCamera = target.GetComponentInChildren<Camera>().transform;
            target.transform.Find("Head").gameObject.SetActive(false);
            transform.SetParent(targetCamera);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            _spectatorUIInitializer.Init(targetNetId);
        }
    }
}
