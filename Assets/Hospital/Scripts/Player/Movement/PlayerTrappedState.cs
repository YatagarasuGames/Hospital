using Mirror;
using UnityEngine;
public class PlayerTrappedState
{
    [SyncVar] public bool isTrapped = false;
    private Rigidbody _rigidbody;
    
    public PlayerTrappedState(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }

    [Server]
    public void TrapPlayer()
    {
        _rigidbody.isKinematic = true;
        isTrapped = true;
        RpcUpdateTrappedState(true);
    }

    [Server]
    public void CmdUnTrapPlayer()
    {
        _rigidbody.isKinematic = false;
        RpcUpdateTrappedState(false);
    }

    [ClientRpc] // Вызывается сервером, но выполняется на всех клиентах
    private void RpcUpdateTrappedState(bool trapped)
    {
        _rigidbody.isKinematic = trapped;
        Debug.Log("tretertq");
    }
}
