using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRotater : MonoBehaviour
{
    [SerializeField] private float _rotateAngle;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Transform _transform;

    private float _rotationTarget;
    private float _defaultRotation;
    private bool _canRotate=true;

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
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.Euler(0,_rotationTarget,0), _rotationSpeed * Time.fixedDeltaTime);
    }

    private float GetTargetRotate()
    {
        float targetRotation = _defaultRotation;
        float direction = _input.Direction;

        if (direction > 0)
            targetRotation = _defaultRotation + _rotateAngle;
        else if (direction < 0)
            targetRotation = _defaultRotation - _rotateAngle;
        else 
            targetRotation = _defaultRotation;

        return targetRotation;
    }

    public void StopRotate()
    {
        _canRotate = false;
        _transform.rotation = Quaternion.Euler(0, _defaultRotation, 0);
    }

    public void StartRotate()
    {
        _canRotate = false;
    }

    public void EndRotate()
    {
        _defaultRotation = _transform.eulerAngles.y;
        _canRotate = true;
    }
}
