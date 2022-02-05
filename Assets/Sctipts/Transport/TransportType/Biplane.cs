using System.Collections;
using UnityEngine;

public class Biplane : Transport
{
    [SerializeField] private PlayerInput _input;

    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _wind;
    [SerializeField] private ParticleSystem _rotate;
    [SerializeField] private Vector3 _currentRoadDirection;
    private float _minHorizontalPosition;
    private float _maxHorizontalPosition;
    private IEnumerator _move;

    public override void StartMove()
    {
        _move = Move();
        StartCoroutine(_move);
        StartCoroutine(ShowWindEffect());
        _animator.enabled = true;

        _rotate.gameObject.SetActive(true);
        _rotate.Play();

        if (_currentRoadDirection.x == 1)
        {
            _minHorizontalPosition = transform.position.z - 0.1f;
            _maxHorizontalPosition = transform.position.z + 6f;
        }
        else if (_currentRoadDirection.z == 1)
        {
            _minHorizontalPosition = transform.position.x - 6f;
            _maxHorizontalPosition = transform.position.x + 0.1f;
        }
        else if (_currentRoadDirection.z == -1)
        {
            _minHorizontalPosition = transform.position.x - 0.1f;
            _maxHorizontalPosition = transform.position.x + 6f;
        }
    }

    public override void StopMove()
    {
        StopCoroutine(_move);
        _forwardSpeed = 0;
        _wind.gameObject.SetActive(false);
        _animator.enabled = false;

        _rotate.Stop();
        _rotate.gameObject.SetActive(false);
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.5f);
        _animator.enabled = false;

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
            transform.Rotate(12,0,currentHorizontalDirection * 30);
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

    private IEnumerator ShowWindEffect()
    {
        yield return new WaitForSeconds(0.2f);
        _wind.gameObject.SetActive(true);
        _wind.Play();
    }
}