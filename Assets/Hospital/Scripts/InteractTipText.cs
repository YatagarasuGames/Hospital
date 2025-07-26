using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Text 3D UI object using TMP_Text
/// </summary>
public class InteractTipText : InteractTip
{
    [SerializeField, TextArea(2, 3)] private string _content;
    public override void Init(Transform interacter)
    {
        base.Init(interacter);
        GetComponentInChildren<TMP_Text>().text = _content;
    }

    private void LateUpdate()
    {
        RotateToPlayer();
    }
}
