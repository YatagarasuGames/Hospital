
using UnityEngine;

public class UITask : UI
{

    public override void LeaveTask()
    {
        _uiCreator.GetComponent<UiTaskGiver>().CmdHandleLeaveTask();
        base.LeaveTask();
    }
    protected void CompleteTask()
    {
        _uiCreator.GetComponent<UiTaskGiver>().CmdHandleTaskCompleted();
    }
}
