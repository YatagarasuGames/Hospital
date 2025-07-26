using Mirror;
using UnityEngine;
public class PlayerTrappedState
{
    private bool _isTrapped = false;
    private Rigidbody _rigidbody;
    
    public PlayerTrappedState(Rigidbody rigidbody, ref bool isTrapped)
    {
        _rigidbody = rigidbody;
        _isTrapped = isTrapped;
    }

    [Server]
    public void TrapPlayer()
    {
        _isTrapped = true;
        RpcUpdateTrappedState(true);
    }

    [Server]
    public void UnTrapPlayer()
    {
        RpcUpdateTrappedState(false);
    }

    [ClientRpc] // Вызывается сервером, но выполняется на всех клиентах
    private void RpcUpdateTrappedState(bool trapped)
    {
        _rigidbody.isKinematic = trapped;
        Debug.Log("tretertq");
    }
}
