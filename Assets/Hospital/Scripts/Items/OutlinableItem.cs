using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkOutline))]
public class OutlinableItem : NetworkBehaviour
{
    [field: SerializeField] public NetworkOutline Outline { get; private set; }
    private void OnEnable()
    {
        Outline.OutlineWidth = 3;
        Outline.OutlineColor = Color.gray;
    }



}
