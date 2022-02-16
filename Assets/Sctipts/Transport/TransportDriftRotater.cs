using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportDriftRotater : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Transform _transform;
    [SerializeField] private float _driftSpeed;
    [SerializeField] private float _driftDuration;
    [SerializeField] private float _driftAngle;


    private float _driftDirection;
    private float _defaultRotation;
    private bool _canRotate = true;
    private Vector3 _lastPosition;
    private Vector3 _currentPosition;
    private IEnumerator _drift;


    private void Start()
    {
        _defaultRotation = _transform.rotation.eulerAngles.y;
        _lastPosition = transform.position;

        _drift = Drift();
    }

    private void FixedUpdate()
    {
        _driftDirection = GetDriftDirection();
        _currentPosition = transform.position;

        if (_lastPosition.z > _currentPosition.z | _lastPosition.z< _currentPosition.z)
        {
            StopCoroutine(_drift);
            _drift = Drift();

            StartCoroutine(_drift);
            _lastPosition = _currentPosition;
        }
    }


    private float GetDriftDirection()
    {
        float direction = _input.Direction;

        if (direction > 0)
            return 1;
        else if (direction < 0)
            return -1;
        else
            return 0;
    }

    private IEnumerator Drift()
    {
        float targetRotatioin = _defaultRotation+_driftDirection*_driftAngle;

        float elapsedTime = 0;
        while (elapsedTime < _driftDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime /_driftDuration;

            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.Euler(0, targetRotatioin, 0), _driftSpeed * Time.deltaTime);
            yield return null;
        }

        elapsedTime = 0;
        while (elapsedTime < _driftDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _driftDuration;

            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.Euler(0, _defaultRotation, 0), _driftSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator RotateToDefault()
    {
        float elapsedTime = 0;

        while (elapsedTime < _driftDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / _driftDuration;

            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.Euler(0, _defaultRotation , 0), _driftSpeed * Time.deltaTime);
            yield return null;
        }
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
}
