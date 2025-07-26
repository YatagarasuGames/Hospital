using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationTaskController : UITask
{
    [SerializeField] private List<Rotator> _rotators;

    [SerializeField] private List<Material> _materials;
    [SerializeField] private List<Image> _borders;
    [SerializeField] private List<string> _materialsNames;

    public override void Update()
    {
        base.Update();
    }
    private void Start()
    {
        _materials.Shuffle();
        for (int i = 0; i < _materials.Count; i++)
        {
            _borders[i].material = _materials[i];
            _materialsNames.Add(_materials[i].name);
        }

        foreach (Rotator rotator in _rotators)
        {
            rotator.Init(_materials);
        }
    }
    public void CheckRotator(Rotator rotator)
    {
        for (int i = 0; i < rotator.MaterialsNameCurrent.Count; i++)
        {
            if (_materialsNames[i] != rotator.MaterialsNameCurrent[i]) return;
        }
        rotator.Complete();
        CheckTaskCompleted();
    }

    private void CheckTaskCompleted()
    {
        foreach (Rotator rotator in _rotators)
        {
            if (!rotator.IsCompleted) return;
        }
        CompleteTask();
    }
}
