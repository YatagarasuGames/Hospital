using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePullLeverController : NetworkBehaviour
{
    [SerializeField] private List<TimePullLever> _levers;
    [SyncVar] private bool _isCompleted = false;
    public void HandleLeverPulled()
    {
        foreach (var lever in _levers)
        {
            if (!lever.IsPulled) return;
        }

        _isCompleted = true;
        foreach (var lever in _levers)
        {
            lever.SetAsCompleted();
        }
        print("Lever Task Completed");
    }
}
