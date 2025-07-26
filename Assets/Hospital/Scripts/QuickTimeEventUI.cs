using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTimeEventUI : UI
{
    [SerializeField] private RectTransform _triggerZone;
    [SerializeField] private RectTransform _fullZone;
    [SerializeField] private RectTransform _cursor;
    [SerializeField] private float _cursorSpeed = 2f;

    [SerializeField] private RectTransform _topPosition;
    [SerializeField] private RectTransform _downPosition;

    private bool _completedNormally = false;

    public override void Init(GameObject uiCreator)
    {
        base.Init(uiCreator);
        InitTriggerZone();
    }

    private void InitTriggerZone()
    {
        float concentration = NetworkClient.localPlayer.GetComponent<IConcentrationProvider>().GetConcentration();
        float newHeight = Mathf.Clamp(Random.Range(50 * concentration, 200 * concentration), 10, _fullZone.sizeDelta.y);
        _triggerZone.sizeDelta = new Vector2(_triggerZone.sizeDelta.x, newHeight);
    }

    public override void Update()
    {
        float time = Mathf.PingPong(Time.time * _cursorSpeed, 1f);
        _cursor.transform.position = Vector3.Lerp(_topPosition.position, _downPosition.position, time);

        if(Input.GetKeyDown(KeyCode.R))
        {
            if (_cursor.anchoredPosition.y < _triggerZone.anchoredPosition.y + _triggerZone.sizeDelta.y / 2 &&
                _cursor.anchoredPosition.y > _triggerZone.anchoredPosition.y - _triggerZone.sizeDelta.y / 2)
            {
                print("Pass");
                Complete(true);
            }
            else
            {
                print("Miss");
                Complete(false);
            }
        }
    }
    private void Complete(bool result)
    {
        _completedNormally = true;
        _uiCreator.GetComponent<QuickTimeEventTask>().CmdHandleQuickTimeEventCompleted(result);
        LeaveTask();
    }

    private void OnDestroy()
    {
        if(_completedNormally == false) _uiCreator.GetComponent<QuickTimeEventTask>().CmdHandleQuickTimeEventCompleted(false);
    }


}
