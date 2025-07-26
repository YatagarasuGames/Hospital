using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SequencePressTaskController : NetworkBehaviour
{
    [SerializeField] private List<SequencePressTaskComponent> _elements;
    private readonly SyncList<int> _componentsPressOrder = new SyncList<int>{ 2, 1, 0};

    [SyncVar] private int _currentlySuccessfullyPressedComponents = 0;

    [Server]
    private void OnEnable()
    {
        for(int i = 0; i<_elements.Count; i++)
        {
            _componentsPressOrder.Add(i);
        }
        _componentsPressOrder.Shuffle();
        for (int i = 0; i < _elements.Count; i++)
        {
            _elements[i].Init(_componentsPressOrder[i]);
        }
    }

    [Server]
    public void HandleComponentPressed(int pressedComponentOrder)
    {
        if (pressedComponentOrder == _componentsPressOrder[_currentlySuccessfullyPressedComponents]) _currentlySuccessfullyPressedComponents++;
        else _currentlySuccessfullyPressedComponents = 0;

        if (_elements.Count == _currentlySuccessfullyPressedComponents) print("цннннннк");
    }
}
