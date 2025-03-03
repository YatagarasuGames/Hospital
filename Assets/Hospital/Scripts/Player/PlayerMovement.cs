using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Animator _animator;
    private Rigidbody _rb;

    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    private void OnEnable()
    {
        _rb ??= GetComponent<Rigidbody>();
    }

    private void Move()
    {
        if (_rb.isKinematic) return;
        _animator.SetBool("Walking", _rb.velocity.magnitude >= 0.2f);
        if (speedOverrides.Count > 0)
        {
            _speed = speedOverrides[speedOverrides.Count - 1]();
        }
        Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal") * _speed, Input.GetAxis("Vertical") * _speed);
        Vector3 movementDirection3 = transform.rotation * new Vector3(movementDirection.x, _rb.velocity.y, movementDirection.y);
        if (movementDirection3.magnitude > _speed) movementDirection3 = movementDirection3.normalized * _speed;
        _rb.velocity = movementDirection3;


    }

    private void FixedUpdate()
    {
        Move();
    }

}