using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordFolder : LocalUICreator
{
    [field: SerializeField] public PasswordTaskData PasswordData { get; private set; }

    public void SetPasswordData(PasswordTaskData passwordData)
    {
        PasswordData = passwordData;
    }
}
