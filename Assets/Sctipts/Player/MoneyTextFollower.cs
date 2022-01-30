using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTextFollower : MonoBehaviour
{
    [SerializeField] private Player _target;

    Vector3 _defaultPosition;
    private float _currentYRotation = 0;

    private void OnEnable()
    {
        _target.ChangedCoinsVisualizatorPosition += StartUseTransport;
        _target.EndUseTransport += SetDefaultPosition;
        _target.RotateZoneEntered += OnPlayerRotated;
        _defaultPosition = transform.localPosition;
    }

    private void OnDisable()
    {
        _target.RotateZoneEntered -= OnPlayerRotated;
        _target.ChangedCoinsVisualizatorPosition -= StartUseTransport;
        _target.EndUseTransport -= SetDefaultPosition;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, _currentYRotation, 0);
    }

    private void OnPlayerRotated(RotateZone zone)
    {
        StartCoroutine(Rotate(zone.RotateDirection));
    }

    private void StartUseTransport(Vector3 transportTextPosition)
    {
        transform.position = transportTextPosition;
    }

    private void SetDefaultPosition()
    {
       transform.localPosition = _defaultPosition;
    }

    private IEnumerator Rotate(int diretion)
    {
        float targetYRotate = 0;
        
        if(diretion == -1)
        {
            targetYRotate = _currentYRotation - 90;
        }
        else
        {
            targetYRotate = _currentYRotation + 90;
        }

        while(_currentYRotation != targetYRotate)
        {
            _currentYRotation = Mathf.MoveTowards(_currentYRotation, targetYRotate, 0.75f);
            yield return null;
        }
    }
}
