using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkOutline))]
public class OutlinableItem : NetworkBehaviour
{
    private NetworkOutline _outline;
    private void OnEnable()
    {
        _outline = GetComponent<NetworkOutline>();
        _outline.CmdDisableOutlineToAll();
        _outline.OutlineWidth = 3;
        _outline.OutlineColor = Color.gray;
    }
    [Server]
    public void EnableOutlineLocal() => _outline.CmdEnableOutlineLocal();
    [Server]
    public void DisableOutlineLocal() => _outline.CmdDisableOutlineLocal();

    [Server]
    public void EnableOutlineToAll() => _outline.CmdEnableOutlineToAll();
    [Server]
    public void DisableOutlineToAll() => _outline.CmdDisableOutlineToAll();


}
