using UnityEngine;

public class DeactivateElectricityTask : UiTaskGiver
{
    [SerializeField] private Door _door;

    public override void CmdHandleTaskCompleted()
    {
        base.CmdHandleTaskCompleted();
        if(_door) _door.Unlock();
        print("");
    }

}
