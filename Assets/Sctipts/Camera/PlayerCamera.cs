using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _camera;
    [SerializeField] [Range(0f, 1f)] private float _moveSpeed;
    [SerializeField] private float _cutSceneDuration;
    [SerializeField] [Min(0.1f)] private float _turnDuration;

    private bool _needFollow = true;
    private Vector3 _offset;
    private Vector3 _currentDirection;
    private IEnumerator _rotate;

    private void OnEnable()
    {
        _player.Failed += OnPlayerFailed;
        _player.ShowCutScene += ShowCutScene;
        _player.RotateZoneEnded += OnPlayerRotate;

        transform.position = _player.transform.position;
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
            transform.position = Vector3.Lerp(transform.position, _player.transform.position, _moveSpeed);
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
            
            _camera.LookAt(_player.transform);

            yield return null;
        }
    }

    private void OnPlayerRotate(RotateZone zone)
    {
        _rotate = Rotate(zone.RotateDirection);

        StartCoroutine(_rotate);

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
    }
}