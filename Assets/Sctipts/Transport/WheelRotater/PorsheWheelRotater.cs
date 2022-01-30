using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorsheWheelRotater : MonoBehaviour
{

    [SerializeField] private float _rotateAngle;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Transform _transform;
    [SerializeField] private bool _isBackWheel;

    private float _rotationTarget;
    private float _defaultRotation;
    private bool _canRotate = true;

    private void Start()
    {
        _defaultRotation = _transform.rotation.eulerAngles.y;
    }

    private void FixedUpdate()
    {
        if (_canRotate)
        {
            Rotate();
        }
    }

    private void Rotate()
    {
        _rotationTarget = GetTargetRotate();
        _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.Euler(0, _rotationTarget, 0), _rotationSpeed * Time.deltaTime);
    }

    private float GetTargetRotate()
    {
        float targetRotation = _defaultRotation;
        float direction = _input.Direction;

        if (_isBackWheel)
        {
            if (direction > 0)
                targetRotation = _defaultRotation - _rotateAngle;
            else if (direction < 0)
                targetRotation = _defaultRotation + _rotateAngle;
            else
                targetRotation = _defaultRotation;
        }
        else
        {
            if (direction > 0)
                targetRotation = _defaultRotation + _rotateAngle;
            else if (direction < 0)
                targetRotation = _defaultRotation - _rotateAngle;
            else
                targetRotation = _defaultRotation;
        }

        return targetRotation;
    }

    public void StopRotate()
    {
        _canRotate = false;
        _transform.rotation = Quaternion.Euler(0, _defaultRotation, 0);
    }

    public void StartRotate()
    {
        _canRotate = true;
    }

    public void RoadRotate()
    {
        _defaultRotation = _transform.eulerAngles.y;
        StartRotate();
    }
}