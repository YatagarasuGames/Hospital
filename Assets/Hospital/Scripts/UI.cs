using Mirror;
using UnityEngine;

/// <summary>
/// <para>Base class for NOT network UI objects.</para>
/// <para>Base Update() call UI delete on ESC button.</para>
/// </summary>
public abstract class UI : MonoBehaviour
{
    protected GameObject _uiCreator;
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveTask();
        }
    }

    /// <summary>
    /// Write initialization of UI logic here.
    /// </summary>
    /// <param name="uiCreator">GameObject from where this UI was called to create</param>
    public virtual void Init(GameObject uiCreator)
    {
        _uiCreator = uiCreator;
    }

    /// <summary>
    /// Call CloseUI() method in PlayerUIDrawer. Use for clearense of UI.
    /// </summary>
    public virtual void LeaveTask()
    {
        GetComponentInParent<PlayerUIDrawer>().CloseUI();
    }

}
