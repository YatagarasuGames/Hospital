using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElectricityTipUi : InteractTipUI
{
    [SerializeField] private Image _progress;
    public override void Init(Transform interacter)
    {
        base.Init(interacter);

    }

    public override void UpdateProgress(float newProgress)
    {
        _progress.fillAmount = newProgress;
    }

    private void LateUpdate()
    {
        RotateToPlayer();
    }
}
