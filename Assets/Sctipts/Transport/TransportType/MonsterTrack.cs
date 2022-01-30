using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrack : Transport
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Animator _animator;
    [SerializeField] private Wheel[] _wheels;
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private MonsterTrackWheelRotater[] _wheelRotaters;


    [SerializeField] private ParticleSystem[] _dirtWeelParticleSystems;

    private float _minHorizontalPosition;
    private float _maxHorizontalPosition;
    private IEnumerator _move;
    private MovementRotater _rotater;


    public override void StartMove()
    {
        _rotater = GetComponent<MovementRotater>();
        _rotater.enabled = true;

        foreach(var wheelRotater in _wheelRotaters)
        {
            wheelRotater.enabled = true;
        }

        _move = Move();
        StartCoroutine(_move);

        foreach(var wheel in _wheels)
        {
            wheel.StartRotate();
            wheel.MoveOnDirt();
        }

        foreach (var wheelDirtEffect in _dirtWeelParticleSystems)
        {
            wheelDirtEffect.Play();
        }

        if (_moveDirection.x == 1)
        {
            _minHorizontalPosition = transform.position.z - 4.5f;
            _maxHorizontalPosition = transform.position.z + 0.1f;
        }
        else if (_moveDirection.z == 1)
        {
            _minHorizontalPosition = transform.position.x - 0.1f;
            _maxHorizontalPosition = transform.position.x + 4.5f;
        }
        else if (_moveDirection.z == -1)
        {
            _minHorizontalPosition = transform.position.x - 4.5f;
            _maxHorizontalPosition = transform.position.x + 0.1f;
        }
    }

    public override void StopMove()
    {
        StopCoroutine(_move);
        _speed = 0;

        foreach (var wheel in _wheels)
        {
            wheel.StopRotate();
        }

        foreach (var wheel in _dirtWeelParticleSystems)
        {
            wheel.gameObject.SetActive(false);
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

        float defaultHeight = transform.position.y;

        while (true)
        {
            float horizontalDirection = _input.InputDirection();

            float targetHorizontalPosition = 0;

            if (_moveDirection.x == 1)
                targetHorizontalPosition = transform.position.z + -horizontalDirection * _horizontalSpeed;
            else if (_moveDirection.z == 1)
                targetHorizontalPosition = transform.position.x - -horizontalDirection * _horizontalSpeed;
            else if (_moveDirection.z == -1)
                targetHorizontalPosition = transform.position.x - horizontalDirection * _horizontalSpeed;

            targetHorizontalPosition = GetTargetZPosition(targetHorizontalPosition);

            Vector3 targetPosition = new Vector3();

            if (_moveDirection.x == 1)
                targetPosition = new Vector3(transform.position.x + _speed , defaultHeight,  targetHorizontalPosition );
            else if (_moveDirection.z == 1)
                targetPosition = new Vector3( targetHorizontalPosition , defaultHeight, transform.position.z + _speed );
            else if (_moveDirection.z == -1)
                targetPosition = new Vector3( targetHorizontalPosition , defaultHeight, transform.position.z - _speed );

            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.0056f * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    private float GetTargetZPosition(float targetHorizontalPosition)
    {
        if (_moveDirection.x == 1)
        {
            if (transform.position.z > _maxHorizontalPosition)
                targetHorizontalPosition = transform.position.z-_horizontalSpeed*Time.deltaTime;
            else if (transform.position.z < _minHorizontalPosition)
                targetHorizontalPosition = transform.position.z+ _horizontalSpeed * Time.deltaTime;
        }
        else if (_moveDirection.z == 1)
        {
            if (transform.position.x > _maxHorizontalPosition)
                targetHorizontalPosition = transform.position.x - _horizontalSpeed * Time.deltaTime;
            else if (transform.position.x < _minHorizontalPosition)
                targetHorizontalPosition = transform.position.x + _horizontalSpeed * Time.deltaTime;
        }
        else if (_moveDirection.z == -1)
        {
            if (transform.position.x > _maxHorizontalPosition)
                targetHorizontalPosition = transform.position.x - _horizontalSpeed * Time.deltaTime;
            else if (transform.position.x < _minHorizontalPosition)
                targetHorizontalPosition = transform.position.x + _horizontalSpeed * Time.deltaTime;
        }

        return targetHorizontalPosition;
    }
}
