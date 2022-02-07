using System.Collections;
using UnityEngine;

public class Flippers : Transport
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _waterEffect;
    [SerializeField] private Vector3 _currentRoadDirection;
    [SerializeField] private Transform _swimPositon;
    [SerializeField] private ParticleSystem _dustEffect;
    [SerializeField] private GameObject _flippersModel;

    [SerializeField] private GameObject[] _flippersForPlayerTemplate;

    private IEnumerator _move;

    private float _minHorizontalPosition;
    private float _maxHorizontalPosition;


    public override void StartMove()
    {
        _player.SetFlippers(0, _flippersForPlayerTemplate[0]);
        _player.SetFlippers(1, _flippersForPlayerTemplate[1]);

        _move = Move();
        StartCoroutine(_move);

        _waterEffect.gameObject.SetActive(true);
        _waterEffect.Play();


        _dustEffect.gameObject.SetActive(true);
        _dustEffect.Play();

        transform.position = _swimPositon.position;

        _flippersModel.gameObject.SetActive(false);



        if (_currentRoadDirection.x == 1)
        {
            _minHorizontalPosition = transform.position.z - 0.2f;
            _maxHorizontalPosition = transform.position.z + 5.5f;
        }
        else if (_currentRoadDirection.z == 1)
        {
            _minHorizontalPosition = transform.position.x - 5.5f;
            _maxHorizontalPosition = transform.position.x + 0.2f;
        }
        else if (_currentRoadDirection.z == -1)
        {
            _minHorizontalPosition = transform.position.x - 0.2f;
            _maxHorizontalPosition = transform.position.x + 5.5f;
        }
    }

    public override void StopMove()
    {
        _player.RemoveFlippers();
        StopCoroutine(_move);
        _forwardSpeed = 0;

        _waterEffect.Stop();
        _waterEffect.gameObject.SetActive(false);
    }

    private IEnumerator Move()
    {
        _input.transform.localRotation = Quaternion.Euler(90,0,90);
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
            transform.Rotate(0,0,-90);
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
