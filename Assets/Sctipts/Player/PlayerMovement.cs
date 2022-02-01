using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _roadWeight;
    [SerializeField] private float _speed;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private Player _player;

    //[SerializeField] private float _minZPosition;
    // [SerializeField] private float _maxZPosition;

    [SerializeField] private MovementRotater _rotater;

    [SerializeField] private AnimationCurve _hitJumpCurve;
    [SerializeField] private float _hitJumpDuration;
    [SerializeField] private float _zHitSpeed;
    [SerializeField] private float _yHitSpeed;

    [SerializeField] private float _finishMovementSpeed;
    // [SerializeField] private float _minDistanseToMove;
    // [SerializeField] private GameObject _camera;

    [SerializeField] private float _testSpeed;
    [SerializeField] private bool _Test;

    [SerializeField] private float _speedInclinedSurfaceMove;

    private float _minHorizontalPosition;
    private float _maxHorizontalPosition;
    private float _targetYPosition;
    private float _direction;

    private bool _canMove = false;
    private IEnumerator _hitMove;
    private IEnumerator _exceptionSelectedTransport;
    private IEnumerator _rotateToFinish;
    private Vector3 _currentMoveDirection;

    public event UnityAction Finished;

    private void OnEnable()
    {
        _horizontalSpeed *= 2;

        _player.StopMove += StopMove;
        _player.EndUseTransport += StartMove;
        _player.Finished += StopMove;
        _player.Failed += Failed;
        _player.Hitted += HitJump;
        _player.ExceptionSelectedTransport += ExceptionSelectedTransport;
        _player.FinishZoneSelected += FinishZoneSelected;
        _player.RotateZoneEntered += OnRotateZoneEntered;
        _player.RotateZoneEnded += OnRotateEnded;
        _player.InclinedSurfaceCollided += OnInclinedSurfaceCollided;
        _player.CollideWithBarrierWall += OnPlayerHitWithBarrier;

        if (_Test)
            _horizontalSpeed = _testSpeed;

        _currentMoveDirection = new Vector3(1, 0, 0);
        _targetYPosition = transform.position.y;
        _minHorizontalPosition = transform.position.z - _roadWeight;
        _maxHorizontalPosition = transform.position.z + _roadWeight;
    }

    private void OnDisable()
    {
        _player.StopMove -= StopMove;
        _player.EndUseTransport -= StartMove;
        _player.Finished -= StopMove;
        _player.Failed -= Failed;
        _player.Hitted -= HitJump;
        _player.CollideWithBarrierWall -= OnPlayerHitWithBarrier;
        _player.ExceptionSelectedTransport -= ExceptionSelectedTransport;
        _player.FinishZoneSelected -= FinishZoneSelected;
        _player.RotateZoneEntered -= OnRotateZoneEntered;
        _player.RotateZoneEnded -= OnRotateEnded;
        _player.InclinedSurfaceCollided -= OnInclinedSurfaceCollided;
    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        _direction = _playerInput.InputDirection();

        float targetHorizontalPosition = -_direction * _horizontalSpeed;

        targetHorizontalPosition = GetTargetZPosition(targetHorizontalPosition);

        Vector3 targetPosition = transform.position;

        if (_currentMoveDirection.x == 1)
        {
            targetPosition = new Vector3(transform.position.x + _speed * Time.fixedDeltaTime, _targetYPosition,
                transform.position.z + targetHorizontalPosition * Time.fixedDeltaTime);
        }
        else if (_currentMoveDirection.z == -1)
        {
            targetPosition = new Vector3(transform.position.x + targetHorizontalPosition * Time.fixedDeltaTime,
                _targetYPosition, transform.position.z - _speed * Time.fixedDeltaTime);
        }
        else if (_currentMoveDirection.z == 1)
        {
            targetPosition = new Vector3(transform.position.x - targetHorizontalPosition * Time.fixedDeltaTime,
                _targetYPosition, transform.position.z + _speed * Time.fixedDeltaTime);
        }

        transform.position = targetPosition;

        //transform.localPosition = Vector3.Lerp(transform.position, targetPosition, 0.01f);
        //transform.localPosition = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.fixedDeltaTime);
    }

    private float GetTargetZPosition(float zPosition)
    {
        if (_currentMoveDirection.x == 1)
        {
            if (transform.position.z > _maxHorizontalPosition)
                zPosition = -_horizontalSpeed;
            else if (transform.position.z < _minHorizontalPosition)
                zPosition = _horizontalSpeed;
        }
        else if (_currentMoveDirection.z == 1)
        {
            if (transform.position.x > _maxHorizontalPosition)
                zPosition = _horizontalSpeed;
            else if (transform.position.x < _minHorizontalPosition)
                zPosition = -_horizontalSpeed;
        }
        else if (_currentMoveDirection.z == -1)
        {
            if (transform.position.x > _maxHorizontalPosition)
                zPosition = -_horizontalSpeed;
            else if (transform.localPosition.x < _minHorizontalPosition)
                zPosition = _horizontalSpeed;
        }

        return zPosition;
    }

    public void StopMove()
    {
        _canMove = false;
        _rotater.enabled = false;
    }

    public void StartMove()
    {
        _canMove = true;
        _rotater.enabled = true;
    }

    private void Failed()
    {
        StartCoroutine(FailedJump());
    }

    private void OnRotateZoneEntered(RotateZone zone)
    {
        _rotater.StopRotate();
    }

    private void OnRotateEnded(RotateZone zone)
    {
        if (zone.RotateDirection == 1)
        {
            if (_currentMoveDirection.x == 1)
            {
                _currentMoveDirection = new Vector3(0, 0, -1);
                _minHorizontalPosition = transform.position.x - _roadWeight;
                _maxHorizontalPosition = transform.position.x + _roadWeight;
            }
            else if (_currentMoveDirection.z == 1)
            {
                _currentMoveDirection = new Vector3(1, 0, 0);
                _minHorizontalPosition = transform.position.z - _roadWeight;
                _maxHorizontalPosition = transform.position.z + _roadWeight;
            }
        }
        else
        {
            if (_currentMoveDirection.x == 1)
            {
                _currentMoveDirection = new Vector3(0, 0, 1);
                _minHorizontalPosition = transform.position.x - _roadWeight;
                _maxHorizontalPosition = transform.position.x + _roadWeight;
            }
            else if (_currentMoveDirection.z == -1)
            {
                _currentMoveDirection = new Vector3(1, 0, 0);
                _minHorizontalPosition = transform.position.z - _roadWeight;
                _maxHorizontalPosition = transform.position.z + _roadWeight;
            }
        }

        StartMove();
    }

    private void HitJump(int direction)
    {
        if (_hitMove != null)
        {
            StopCoroutine(_hitMove);
        }

        _hitMove = HitMove(direction);
        StartCoroutine(_hitMove);
    }

    private void ExceptionSelectedTransport(Transform targetJumpPosition)
    {
        if (_exceptionSelectedTransport != null)
        {
            StopCoroutine(_exceptionSelectedTransport);
        }

        _exceptionSelectedTransport = ExceptionSelectedTransportMovement(targetJumpPosition);
        StartCoroutine(_exceptionSelectedTransport);
    }

    private void OnPlayerHitWithBarrier()
    {
        StartCoroutine(ThrowBack());
    }


    private IEnumerator HitMove(int direction)
    {
        float elapsedTime = 0;

        while (elapsedTime < _hitJumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _hitJumpDuration;

            if (_currentMoveDirection.x == 1)
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x,
                        _targetYPosition + _hitJumpCurve.Evaluate(progress) * _yHitSpeed * Time.deltaTime,
                        transform.position.z + _zHitSpeed * Time.deltaTime * direction), 0.05f);
            else if (_currentMoveDirection.z == 1)
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x + _zHitSpeed * Time.deltaTime * direction,
                        _targetYPosition + _hitJumpCurve.Evaluate(progress) * _yHitSpeed * Time.deltaTime,
                        transform.position.z), 0.05f);
            else if (_currentMoveDirection.z == -1)
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x - _zHitSpeed * Time.deltaTime * direction,
                        _targetYPosition + _hitJumpCurve.Evaluate(progress) * _yHitSpeed * Time.deltaTime,
                        transform.position.z), 0.05f);

            yield return null;
        }
    }

    private IEnumerator ThrowBack()
    {
        float elapsedTime = 0;

        Vector3 startThrowBackPosition = transform.position;

        while (elapsedTime < _hitJumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _hitJumpDuration;

            if (_currentMoveDirection.x == 1)
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(startThrowBackPosition.x - 5,
                        _targetYPosition + _hitJumpCurve.Evaluate(progress) * _yHitSpeed * Time.deltaTime,
                        transform.position.z), 0.05f);
            else if (_currentMoveDirection.z == 1)
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x,
                        _targetYPosition + _hitJumpCurve.Evaluate(progress) * _yHitSpeed * Time.deltaTime,
                        startThrowBackPosition.z - 5), 0.05f);
            else if (_currentMoveDirection.z == -1)
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x,
                        _targetYPosition + _hitJumpCurve.Evaluate(progress) * _yHitSpeed * Time.deltaTime,
                        startThrowBackPosition.z + 5), 0.05f);

            yield return null;
        }
    }

    private void OnInclinedSurfaceCollided(Transform target)
    {
        StartCoroutine(MoveToTarget(target));
    }

    private void FinishZoneSelected(Transform targetPosition)
    {
        _rotateToFinish = RotateToFinish();
        StartCoroutine(MoveToFinish(targetPosition));
        StartCoroutine(_rotateToFinish);
    }

    private IEnumerator MoveToTarget(Transform inclinedSurfaceTarget)
    {
        while (transform.position.y != inclinedSurfaceTarget.position.y)
        {
            _targetYPosition = Mathf.MoveTowards(_targetYPosition, inclinedSurfaceTarget.position.y,
                _speedInclinedSurfaceMove * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }


    private IEnumerator ExceptionSelectedTransportMovement(Transform targetJumpPosition)
    {
        StopMove();

        float elapsedTime = 0;

        while (elapsedTime < _hitJumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _hitJumpDuration;

            transform.position = Vector3.Lerp(transform.position,
                new Vector3(targetJumpPosition.position.x,
                    _targetYPosition + _hitJumpCurve.Evaluate(progress) * _yHitSpeed * Time.deltaTime,
                    targetJumpPosition.position.z), 0.034f);
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, _targetYPosition, transform.position.z);
        StartMove();
    }

    private IEnumerator FailedJump()
    {
        StopMove();

        float elapsedTime = 0;

        Vector3 transformPosition = transform.position;

        while (elapsedTime < _hitJumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _hitJumpDuration;

            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transformPosition.x - 8,
                    _targetYPosition + _hitJumpCurve.Evaluate(progress) * _yHitSpeed * Time.deltaTime,
                    transform.position.z), 0.05f);
            yield return null;
        }
    }


    private IEnumerator RotateOnFinishZone()
    {
        if (_rotateToFinish != null)
            StopCoroutine(_rotateToFinish);

        StopMove();

        Quaternion finishRotation;

        if (_currentMoveDirection.x == 1)
            finishRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);
        else if (_currentMoveDirection.z == 1)
            finishRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        else
            finishRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);

        while (transform.rotation.eulerAngles.y != finishRotation.eulerAngles.y)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, finishRotation, 0.1f);
            yield return null;
        }


        yield return null;
    }

    private IEnumerator RotateToFinish()
    {
        Quaternion targetFinishRotation = new Quaternion();
        _rotater.enabled = false;
        if (_currentMoveDirection.x == 1)
        {
            targetFinishRotation =
                Quaternion.Euler(transform.rotation.eulerAngles.x, -90, transform.rotation.eulerAngles.z);
        }
        else if (_currentMoveDirection.z == 1)
        {
            targetFinishRotation =
                Quaternion.Euler(transform.rotation.eulerAngles.x, 180, transform.rotation.eulerAngles.z);
        }
        else if (_currentMoveDirection.z == -1)
        {
            targetFinishRotation =
                Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        }


        while (transform.rotation.eulerAngles.y != targetFinishRotation.eulerAngles.y)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetFinishRotation, 0.1f);
            yield return null;
        }
    }

    private IEnumerator MoveToFinish(Transform targetPosition)
    {
        while (transform.position != targetPosition.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position,
                _finishMovementSpeed * Time.deltaTime);
            yield return null;
        }

        Finished?.Invoke();
        StartCoroutine(RotateOnFinishZone());
    }
}