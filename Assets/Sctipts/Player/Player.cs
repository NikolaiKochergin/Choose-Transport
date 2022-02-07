using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Wallet))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private float _exitDelay;
    [SerializeField] private float _exitMoveSpeed;
    [SerializeField] private float _rotateMoveSpeed;
    [SerializeField][Min(0.1f)] private float _rotateDuration;

    [SerializeField] private GameObject[] _flippersParents;

    private List<GameObject> _spawnedFlippers = new List<GameObject>();

    private Wallet _wallet;
    private MovementRotater _rotater;

    public event UnityAction<int> CollideWithMoney;
    public event UnityAction<int> CollideWithBarrier;
    public event UnityAction<string> StartedUseTransport;
    public event UnityAction<Vector3> ChangedCoinsVisualizatorPosition;
    public event UnityAction<Transform> FinishZoneSelected;
    public event UnityAction<Transform> ShowCutScene;
    public event UnityAction EndUseTransport;
    public event UnityAction StartedRun;
    public event UnityAction StopMove;
    public event UnityAction Finished;
    public event UnityAction Failed;
    public event UnityAction<RotateZone> RotateZoneEntered;
    public event UnityAction<RotateZone> RotateZoneEnded;
    public event UnityAction<int> TransportPurchased;
    public event UnityAction<int> Hitted;
    public event UnityAction<Transform> ExceptionSelectedTransport;
    public event UnityAction StartedFall;
    public event UnityAction StartedExitFromTransport;
    public event UnityAction StartedFinishedMove;
    public event UnityAction<Transform> InclinedSurfaceCollided;
    public event UnityAction CollideWithBarrierWall;

    public event UnityAction StartExitFromTransport;
    public event UnityAction EndExitFromTransport;

    private int _countException = 0;
    private bool _canUseTransport = true;
    private bool _isUseTransport = false;
    private Vector3 _defaultRotation;
    private IEnumerator _playAnimationWithDelay;
    private IEnumerator _escapeFromTransport;
    private Vector3 _currentDirection = new Vector3(1, 0, 0);
    private const int MaxCountException = 2;

    public bool CanUseTransport => _canUseTransport;
    public bool IsUseTransport => _isUseTransport;

    public int Coins => _wallet.Coins;

    private void OnEnable()
    {        
        _wallet = GetComponent<Wallet>();
        _rotater = GetComponent<MovementRotater>();

        _defaultRotation = transform.rotation.eulerAngles;

        _movement.Finished += OnFinished;
    }

    private void OnDisable()
    {
        _movement.Finished -= OnFinished;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Money>())
        {
            Money money = other.GetComponent<Money>();

            CollideWithMoney?.Invoke(money.Coins);
            money.CollideWithPlayer();
        }
        else if (other.GetComponent<MoneyBarrier>())
        {
            MoneyBarrier barrier = other.GetComponent<MoneyBarrier>();
            Hit(barrier);
        }
        else if (other.GetComponent<RotateZone>())
        {
            RotateZone rotateZone = other.GetComponent<RotateZone>();
            rotateZone.Disable();

            OnRotateZoneEntered(rotateZone);
        }
        else if (other.GetComponent<InclinedSurface>())
        {
            InclinedSurface inclinedSurface = other.GetComponent<InclinedSurface>();
            MoveOnInclinedSurface(inclinedSurface.Target);
            inclinedSurface.TriggerDisable();
        }
        else if (other.GetComponent<BarrierHitBackZone>())
        {
            CollideWithBarrierWall?.Invoke();
            other.GetComponent<BarrierHitBackZone>().Hit();
        }
    }

    public bool IsHaveEnoughMoney(int needCoins)
    {
        if (_wallet.Coins >= needCoins)
            return true;

        return false;
    }

    public void StartUseTransport(Transport transport)
    {
        _isUseTransport = true;

        _countException = 0;
        _playAnimationWithDelay = PlayAnimationWithDelay(transport);
        StartCoroutine(_playAnimationWithDelay);
        
    }

    public void StopUseTransport(Transport transport)
    {
        _isUseTransport = false;
        transform.parent = null;

        _escapeFromTransport = EscapeFromTransport(transport.GetExitPosition());
        StartCoroutine(_escapeFromTransport);
    }

    public void ExceptionUseTransport(Transform _jumpPosition)
    {
        _countException++;

        if (_countException >= MaxCountException)
        {
            _canUseTransport = false;
            Failed?.Invoke();
            StopMove?.Invoke();
        }
        else
        {
            _isUseTransport = false;
            ExceptionSelectedTransport?.Invoke(_jumpPosition);
        }
    }

    public void StartMove()
    {
        EndUseTransport?.Invoke();
        StartedRun?.Invoke();
    }

    public void HitOnTransport(MoneyBarrier barrier)
    {
        Hit(barrier);
    }

    public void EnterOnFinishZone(SelectedZone zone)
    {
        StopMove?.Invoke();
        FinishZoneSelected?.Invoke(zone.PlayerMovementTarget);
        ShowCutScene?.Invoke(zone.CameraTarget);
        StartedFinishedMove?.Invoke();
    }

    public void CollideWithMoneyOnTransport(Money money)
    {
        CollideWithMoney?.Invoke(money.Coins);
    }

    public void FailedOnFinish()
    {
        Failed?.Invoke();
        StopMove?.Invoke();
    }

    public void SetFlippers(int index,GameObject flipperModel)
    {
        var spawnedFlipers =Instantiate(flipperModel, _flippersParents[index].transform);
        _spawnedFlippers.Add(spawnedFlipers);
    }

    public void RemoveFlippers()
    {
        for( int i=0; i<2; i++)
        {
            Destroy(_spawnedFlippers[i]);
        }
        _spawnedFlippers = new List<GameObject>();
    }

    private void OnFinished()
    {
        Finished?.Invoke();
       // Vector3 direction = transform.position - _camera.gameObject.transform.position;
       // Quaternion rotation = Quaternion.LookRotation(direction);
       // transform.rotation = rotation;
    }

    private void OnRotateZoneEntered(RotateZone zone)
    {
        RotateZoneEntered?.Invoke(zone);
        StopMove?.Invoke();
        StartCoroutine(MoveToRotateTarget(zone));
        StopMove();
    }

    private void Hit(MoneyBarrier barrier)
    {        
        barrier.CollideWithPlayer();
        CollideWithBarrier?.Invoke(barrier.CoinsCrush);

        if (barrier.HitJump)
        {
            int direction = barrier.HitJumpDirection;
            Hitted?.Invoke(direction);
        }
    }

    private void MoveOnInclinedSurface(Transform targetPosition)
    {
        InclinedSurfaceCollided?.Invoke(targetPosition);
    }

    private IEnumerator PlayAnimationWithDelay(Transport transport)
    {
        yield return new WaitForSeconds(transport.DelayBeforePlayAnimation);
        StartedUseTransport?.Invoke(transport.NameUseAnimation);
        yield return new WaitForSeconds(transport.Delay);

        StopMove?.Invoke();
        transform.SetParent(transport.PlayerParent);
        transform.position = transport.GetSeetPosition();
        _model.transform.localPosition = new Vector3();
        TransportPurchased?.Invoke(transport.Price);
        ChangedCoinsVisualizatorPosition?.Invoke(transport.PlayerCoinsPosition);
    }

    private IEnumerator EscapeFromTransport(Vector3 exitPosition)
    {
        StartedExitFromTransport?.Invoke();
        StartExitFromTransport?.Invoke();
        //transform.position = new Vector3(transform.position.x,0,transform.position.z);
        if (_currentDirection.x == 1)
        {
            while (transform.position.x != exitPosition.x)
            {
                transform.position = new Vector3(Mathf.MoveTowards(transform.position.x, exitPosition.x, _exitMoveSpeed * Time.deltaTime), transform.position.y, transform.position.z);
                yield return null;
            }
        }
        else if(_currentDirection.z == 1)
        {
            while (transform.position.z != exitPosition.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.MoveTowards(transform.position.z, exitPosition.z, _exitMoveSpeed * Time.deltaTime));
                yield return null;
            }
        }
        else if(_currentDirection.z == -1)
        {
            while (transform.position.z != exitPosition.z)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.MoveTowards(transform.position.z, exitPosition.z, _exitMoveSpeed * Time.deltaTime));
                yield return null;
            }
        }


        EndExitFromTransport?.Invoke();
        _model.transform.localPosition = new Vector3(0, 0, 0);
        StartMove();
        transform.localRotation = Quaternion.Euler(_defaultRotation);
    }

    private IEnumerator MoveToRotateTarget(RotateZone zone)
    {
        if (_currentDirection.x == 1)
        {
           Vector3 targetPosition = new Vector3(zone.EndRotatePosition.position.x, transform.position.y, transform.position.z); 

            while (transform.position.x != zone.EndRotatePosition.position.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _rotateMoveSpeed * Time.deltaTime );
                yield return null;
            }
        }
        else if ( _currentDirection.z == 1|_currentDirection.z == -1)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, zone.EndRotatePosition.position.z);

            while (transform.position.z != zone.EndRotatePosition.position.z)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _rotateMoveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        StartCoroutine(Rotate(zone));
    }

    private IEnumerator Rotate(RotateZone zone )
    {
        float TargetRotation = 0;
        _rotater.StartRotate();

        if (zone.RotateDirection== 1)
        {
            if (_currentDirection.x == 1)
            {
                TargetRotation = 90;
                _currentDirection = new Vector3(0, 0, -1);
            }
            else if (_currentDirection.z == 1)
            {
                TargetRotation = 0;
                _currentDirection = new Vector3(1, 0, 0);
            }
        }
        else if (zone.RotateDirection == -1)
        {
            if (_currentDirection.x == 1)
            {
                TargetRotation = -90;
                _currentDirection = new Vector3(0, 0, 1);
            }
            else if (_currentDirection.z == -1)
            {
                TargetRotation = 0;
                _currentDirection = new Vector3(1, 0, 0);
            }
        }
        RotateZoneEnded?.Invoke(zone);

        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, TargetRotation, transform.eulerAngles.z);

        float rotateSpeed = Quaternion.Angle(targetRotation, transform.rotation) / _rotateDuration;
        float timer = _rotateDuration;
        
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        
        transform.rotation = targetRotation;
        _defaultRotation = transform.rotation.eulerAngles;
        _rotater.EndRotate();



        //yield return null;
    }
}