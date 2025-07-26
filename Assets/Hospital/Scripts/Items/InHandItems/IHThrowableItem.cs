using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class IHThrowableItem : InHandItem
{
    [SerializeField] private GameObject _throwableItem;
    [SerializeField] private float _throwForceOffset;

    [Server]
    public override void Use()
    {
        base.Use();
        Transform camera = GetComponentInParent<ModulesObjectsBuffer>().Camera.transform;
        GameObject throwedItem = Instantiate(_throwableItem, transform.position, Quaternion.identity);
        NetworkServer.Spawn(throwedItem);
        var rb = throwedItem.GetComponent<Rigidbody>();
        rb.AddForce(rb.mass * _throwForceOffset * camera.forward);
        onItemUsed?.Invoke(connectionToClient);
    }
}
