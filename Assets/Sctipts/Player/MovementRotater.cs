using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRotater : MonoBehaviour
{
    [SerializeField] private float _rotateAngle;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Transform _playerModel;

    private float _rotationTarget;
    private float _defaultRotation;
    private bool _canRotate = true;
    
    public Transform PlayerModel => _playerModel;

    private void Start()
    {
        _defaultRotation = _playerModel.rotation.eulerAngles.y;
    }

    private void FixedUpdate()
    {
        if (_canRotate)
        {
            Rotate();
        }
    }

    public void LookAt(Vector3 direction)
    {
        _playerModel.LookAt(_playerModel.position + direction);
    }

    private void Rotate()
    {
        _rotationTarget = GetTargetRotate();
        _playerModel.rotation = Quaternion.RotateTowards(_playerModel.rotation, Quaternion.Euler(0, _rotationTarget, 0),
            _rotationSpeed * Time.fixedDeltaTime);
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
        _playerModel.rotation = Quaternion.Euler(0, _defaultRotation, 0);
    }

    public void StartRotate()
    {
        _canRotate = false;
    }

    public void EndRotate()
    {
        _defaultRotation = _playerModel.eulerAngles.y;
        _canRotate = true;
    }
}