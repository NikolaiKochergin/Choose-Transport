using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flippers : Transport
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _waterEffect;
    [SerializeField] private Vector3 _moveDirection;
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



        if (_moveDirection.x == 1)
        {
            _minHorizontalPosition = transform.position.z - 0.2f;
            _maxHorizontalPosition = transform.position.z + 5.5f;
        }
        else if (_moveDirection.z == 1)
        {
            _minHorizontalPosition = transform.position.x - 5.5f;
            _maxHorizontalPosition = transform.position.x + 0.2f;
        }
        else if (_moveDirection.z == -1)
        {
            _minHorizontalPosition = transform.position.x - 0.2f;
            _maxHorizontalPosition = transform.position.x + 5.5f;
        }
    }

    public override void StopMove()
    {
        _player.RemoveFlippers();
        StopCoroutine(_move);
        _speed = 0;

        _waterEffect.Stop();
        _waterEffect.gameObject.SetActive(false);
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
                targetPosition = new Vector3(transform.position.x + _speed, defaultHeight, targetHorizontalPosition);
            else if (_moveDirection.z == 1)
                targetPosition = new Vector3(targetHorizontalPosition, defaultHeight, transform.position.z + _speed);
            else if (_moveDirection.z == -1)
                targetPosition = new Vector3(targetHorizontalPosition, defaultHeight, transform.position.z - _speed);

            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.0056f * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }

    private float GetTargetZPosition(float targetHorizontalPosition)
    {
        if (_moveDirection.x == 1)
        {
            if (transform.position.z > _maxHorizontalPosition)
                targetHorizontalPosition = transform.position.z - _horizontalSpeed * Time.deltaTime;
            else if (transform.position.z < _minHorizontalPosition)
                targetHorizontalPosition = transform.position.z + _horizontalSpeed * Time.deltaTime;
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
