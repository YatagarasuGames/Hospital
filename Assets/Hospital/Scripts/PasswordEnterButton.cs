using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class PasswordEnterButton : NetworkBehaviour, IPointerClickHandler
{
    public int _number;
    [SerializeField] private EnterPasswordTaskController _taskController;

    public void OnPointerClick(PointerEventData eventData)
    {
        _taskController.HandleKeyPressed(_number);
        print("Clicked");
    }
}
