using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementRotater))]
public class Porsche : Transport
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Animator _animator;
    [SerializeField] private Wheel[] _wheels;
    [SerializeField] private Vector3 _currentRoadDirection;
    [SerializeField] private PorsheWheelRotater[] _wheelRotaters;


    private IEnumerator _move;
    private MovementRotater _rotater;
    private float _minHorizontalPosition;
    private float _maxHorizontalPosition;
    public override void StartMove()
    {
        _rotater = GetComponent<MovementRotater>();
        _rotater.enabled = true;

        foreach (var wheelRotater in _wheelRotaters)
        {
            wheelRotater.enabled = true;
        }

        _move = Move();
        StartCoroutine(_move);

        foreach (var wheel in _wheels)
        {
            wheel.StartRotate();
        }


        if (_currentRoadDirection.x == 1)
        {
            _minHorizontalPosition = transform.position.z - 0.1f;
            _maxHorizontalPosition = transform.position.z + 5.5f;
        }
        else if (_currentRoadDirection.z == 1)
        {
            _minHorizontalPosition = transform.position.x - 5.5f;
            _maxHorizontalPosition = transform.position.x + 0.1f;
        }
        else if (_currentRoadDirection.z == -1)
        {
            _minHorizontalPosition = transform.position.x - 0.1f;
            _maxHorizontalPosition = transform.position.x + 5.5f;
        }
    }

    public override void StopMove()
    {
        StopCoroutine(_move);
        _forwardSpeed = 0;

        foreach (var wheel in _wheels)
        {
            wheel.StopRotate();
        }

        foreach (var wheelRotater in _wheelRotaters)
        {
            wheelRotater.enabled = false;
        }

        _rotater.enabled = false;
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.5f);

        Vector3 currentDirection = Vector3.zero;
        float defaultHeight = transform.position.y;
        float currentHorizontalDirection = 0;

        while (true)
        {
            float targetHorizontalDirection = _input.InputDirection() * -1;

            currentHorizontalDirection = Mathf.MoveTowards(currentHorizontalDirection, targetHorizontalDirection,
                _turnSpeed * Time.fixedDeltaTime);

            if (_currentRoadDirection.x == 1)
            {
                currentDirection = new Vector3(_forwardSpeed, 0, currentHorizontalDirection * _horizontalSpeed) *
                                   Time.fixedDeltaTime;
            }
            else if (_currentRoadDirection.z == -1)
            {
                currentDirection = new Vector3(currentHorizontalDirection * _horizontalSpeed, 0, -_forwardSpeed) *
                                   Time.fixedDeltaTime;
            }
            else if (_currentRoadDirection.z == 1)
            {
                currentDirection = new Vector3(-currentHorizontalDirection * _horizontalSpeed, 0, _forwardSpeed) *
                                   Time.fixedDeltaTime;
            }

            transform.position = new Vector3(transform.position.x, defaultHeight, transform.position.z) +
                                 currentDirection;
            
            transform.LookAt(transform.position + currentDirection);
            //transform.Rotate(0,90,0);
            ClampPlayerMovement();

            yield return new WaitForFixedUpdate();
        }
    }

    private void ClampPlayerMovement()
    {
        if (_currentRoadDirection.x == 1 || _currentRoadDirection.x == -1)
        {
            float zPosition = Mathf.Clamp(transform.position.z, _minHorizontalPosition, _maxHorizontalPosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
        }
        else
        {
            float xPosition = Mathf.Clamp(transform.position.x, _minHorizontalPosition, _maxHorizontalPosition);
            transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
        }
    }
}
