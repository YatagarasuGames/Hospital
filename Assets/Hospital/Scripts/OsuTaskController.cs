using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsuTaskController : UITask
{
    [SerializeField] private int _maxRoundsCount;
    private int _roundNumber = 0;
    private int _nextCirlceIndexToPress;
    [SerializeField] private int _circlesPerRound;
    [SerializeField] private GameObject _circle;
    [SerializeField] private List<Transform> _circlesPositions;
    private List<OsuTaskCircle> _circles = new List<OsuTaskCircle>();

    public override void Update()
    {
        base.Update(); 
    }

    private void OnEnable()
    {
        _roundNumber = 0;
        CreateRound();
    }

    private void CreateRound()
    {
        _roundNumber++;
        if (_roundNumber > _maxRoundsCount) { FinishTask(); return; }
        _circles.Clear();

        for(int i = _circlesPerRound; i > 0; i--)
        {
            CreateCircle(i);
        }
    }
    private void CreateCircle(int circlePressIndex)
    {
        GameObject tempCirlce = Instantiate(_circle, _circlesPositions[Random.Range(0, _circlesPositions.Count)]);
        var tempCirlceComponent = tempCirlce.GetComponent<OsuTaskCircle>();
        tempCirlceComponent.Init(circlePressIndex, this);
        _circles.Add(tempCirlceComponent);
        _nextCirlceIndexToPress = circlePressIndex;
    }

    public bool HandleCirclePressed(int pressedCircleIndex)
    {
        if(pressedCircleIndex == _nextCirlceIndexToPress)
        {
            _nextCirlceIndexToPress += 1;
            if (_nextCirlceIndexToPress > _circlesPerRound) { _circles.Clear(); CreateRound(); }
            return true;
        }
        return false;
    }

    private void FinishTask()
    {
        CompleteTask();
    }
}
