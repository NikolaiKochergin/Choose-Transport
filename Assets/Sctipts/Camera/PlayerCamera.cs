using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private const float s_speedMultiplier = 10f;
    
    [SerializeField] private Player _player;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _cutSceneDuration;
    [SerializeField] private float _cutSceneSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] [Min(0.4f)] private float _turnDuration;
    [SerializeField] private Transform _rotateFollowPosition;

    private bool _needFollow = true;
    private Vector3 _offset;
    private Vector3 _currentDirection;
    private IEnumerator _rotate;

    private void OnEnable()
    {
        _player.Failed += OnPlayerFailed;
        _player.ShowCutScene += ShowCutScene;
        _player.RotateZoneEnded += OnPlayerRotate;

        _offset = _player.transform.position - transform.position;
        _currentDirection = new Vector3(1, 0, 0);
    }

    private void OnDisable()
    {
        _player.Failed -= OnPlayerFailed;
        _player.ShowCutScene -= ShowCutScene;
        _player.RotateZoneEnded -= OnPlayerRotate;
    }

    private void FixedUpdate()
    {
        if (_needFollow)
        {
            Vector3 desiredPosition = _player.transform.position - _offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnPlayerFailed()
    {
        StartCoroutine(WaitOneSecond());
    }

    private IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1);
        _needFollow = false;
    }

    private void ShowCutScene(Transform targetPosition)
    {
        _needFollow = false;
        StartCoroutine(CutSceneMove(targetPosition));
    }

    private IEnumerator CutSceneMove(Transform targetPosition)
    {
        float elapsedTime = 0;
        while (elapsedTime < _cutSceneDuration)
        {
            elapsedTime += Time.deltaTime;

            Vector3 direction = targetPosition.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.04f);

            if (_currentDirection.x == 1)
            {
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x + _cutSceneSpeed, transform.position.y, transform.position.z),
                    _rotateSpeed * Time.fixedDeltaTime);
            }
            else if (_currentDirection.z == 1)
            {
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z + _cutSceneSpeed),
                    _rotateSpeed * Time.fixedDeltaTime);
            }
            else if (_currentDirection.z == -1)
            {
                transform.position = Vector3.Lerp(transform.position,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z - _cutSceneSpeed),
                    _rotateSpeed * Time.fixedDeltaTime);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnPlayerRotate(RotateZone zone)
    {
        _rotate = Rotate(zone.RotateDirection);

        StartCoroutine(_rotate);
        StartCoroutine(MoveToRotatePosition(_rotateFollowPosition));

        if (zone.RotateDirection == 1)
        {
            if (_currentDirection.x == 1)
                _currentDirection = new Vector3(0, 0, -1);
            else if (_currentDirection.z == 1)
                _currentDirection = new Vector3(1, 0, 0);
        }
        else
        {
            if (_currentDirection.x == 1)
                _currentDirection = new Vector3(0, 0, 1);
            else if (_currentDirection.z == -1)
                _currentDirection = new Vector3(1, 0, 0);
        }
    }

    private IEnumerator Rotate(int direction)
    {
        float yTargetRotation;

        if (direction == 1)
            yTargetRotation = transform.rotation.eulerAngles.y + 90;
        else
            yTargetRotation = transform.rotation.eulerAngles.y - 90;

        _needFollow = false;


        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yTargetRotation,
            transform.rotation.eulerAngles.z);

        float rotateSpeed = Quaternion.Angle(transform.rotation, targetRotation) / _turnDuration;
        float currentTime = _turnDuration;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = targetRotation;

        _offset = _player.transform.position - _rotateFollowPosition.position;
        _needFollow = true;
    }

    private IEnumerator MoveToRotatePosition(Transform targetPosition)
    {
        float currentTime = _turnDuration;
        
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position,
                _moveSpeed * s_speedMultiplier * Time.deltaTime);
    
            yield return null;
        }
    
        transform.position = _rotateFollowPosition.position;
    }
}