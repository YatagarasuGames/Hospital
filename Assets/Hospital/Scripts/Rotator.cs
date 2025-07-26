using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Rotator : MonoBehaviour, IPointerClickHandler
{
    public bool IsCompleted { get; private set; } = false;
    [SerializeField] private RotationTaskController _controller;
    [SerializeField] private float _rotationSpeed = 360f;

    [SerializeField] private List<Image> _parts;

    public List<string> MaterialsNameCurrent;

    private bool _isRotating = false;
    private float _targetRotationZ = 0f;
    private float _currentRotationZ = 0f;


    public void Init(List<Material> materials)
    {
        Cursor.lockState = CursorLockMode.Confined;
        for (int i = 0; i < materials.Count; i++)
        {
            _parts[i].material = materials[i];
            MaterialsNameCurrent.Add(materials[i].name);
        }

        for (int i = 1; i < Random.Range(1, 3); i++) RotateInstant();
        _currentRotationZ = transform.localEulerAngles.z;
        _targetRotationZ = _currentRotationZ;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //print("GFHGJG");
        //if(IsCompleted) return;
        //if (!_isRotating)
        //{
        //    _targetRotationZ += 90f;
        //    StartCoroutine(RotateSmoothly());
        //}
    }

    public void Rotate()
    {
        print("GFHGJG");
        if (IsCompleted) return;
        if (!_isRotating)
        {
            _targetRotationZ += 90f;
            StartCoroutine(RotateSmoothly());
        }
    }

    private IEnumerator RotateSmoothly()
    {
        _isRotating = true;

        while (Mathf.Abs(_currentRotationZ - _targetRotationZ) > 0.1f)
        {
            _currentRotationZ = Mathf.MoveTowards(
                _currentRotationZ,
                _targetRotationZ,
                _rotationSpeed * Time.deltaTime);

            transform.localRotation = Quaternion.Euler(0, 0, _currentRotationZ);
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(0, 0, _targetRotationZ);
        SwapMaterialNames();
        _controller.CheckRotator(this);
        _isRotating = false;
    }

    public void RotateInstant()
    {
        if (IsCompleted) return;
        _targetRotationZ += 90f;
        transform.localRotation = Quaternion.Euler(0, 0, _targetRotationZ);
        SwapMaterialNames();
        _controller.CheckRotator(this);
    }

    private void SwapMaterialNames()
    {
        MaterialsNameCurrent.Add(MaterialsNameCurrent[0]);
        MaterialsNameCurrent.RemoveAt(0);

    }

    public void Complete()
    {
        IsCompleted = true;
    }
}
