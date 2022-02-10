using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Transport : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] protected float _forwardSpeed;
    [SerializeField] protected float _horizontalSpeed;
    [SerializeField] protected float _turnSpeed;
    [SerializeField] protected float _delay;
    [SerializeField] protected float _delayBeforeStartUseAnimation;
    [SerializeField] private TransportTrigger _trigger;
    [SerializeField] string _nameOfPlayerUseAnimation;
    [SerializeField] private Transform _seatPosition;
    [SerializeField] protected Transform _exitPosition;
    [SerializeField] private Transform _playerParent;
    [SerializeField] private Transform _jumpPosition;
    [SerializeField] private Transform _playerCoinsPosition;

    [SerializeField] private float _testSpeed;
    [SerializeField] private bool _Test;

    protected Player _player;
    public int Price => _price;
    public float Delay => _delay;
    public float DelayBeforePlayAnimation => _delayBeforeStartUseAnimation;
    public string NameUseAnimation => _nameOfPlayerUseAnimation;
    public Transform PlayerParent => _playerParent;
    public Vector3 PlayerCoinsPosition => _playerCoinsPosition.position;

    public event UnityAction TochBarrierFinish;
    public event UnityAction<Player> UsageCancelled;
    public event UnityAction StartedUse;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EndBarrierZone>())
            Disable();
        else if (other.GetComponent<MoneyBarrier>())
        {
            other.GetComponent<MoneyBarrier>().CollideWithPlayer();
            _player.HitOnTransport(other.GetComponent<MoneyBarrier>());
        }
        else if (other.GetComponent<Money>())
        {
            Money money = other.GetComponent<Money>();
            _player.CollideWithMoneyOnTransport(money);
            money.CollideWithPlayer();
        }
    }

    private void OnEnable()
    {
        _trigger.TransportSelected += OnTransportTriggerEnter;
        _horizontalSpeed *= 2;
        if (_Test)
            _horizontalSpeed = _testSpeed;
    }

    private void OnDisable()
    {
        _trigger.TransportSelected -= OnTransportTriggerEnter;
    }

    private void OnTransportTriggerEnter(Player player)
    {
        _player = player;
        if (_player.CanUseTransport)
        {
            if (player.IsHaveEnoughMoney(_price))
            {
                _trigger.gameObject.SetActive(false);
                player.StartUseTransport(this);
                Activate();
                StartedUse?.Invoke();
            }
            else
            {
                player.ExceptionUseTransport(_jumpPosition);
                UsageCancelled?.Invoke(player);
            }
        }
    }

    public void Activate()
    {
        StartMove();
    }

    public  void Disable()
    {
        _player.StopUseTransport(this);
        StopMove();
    }

    public virtual void StartMove() 
    {
    }

    public virtual void StopMove()
    {
    }

    public Vector3 GetSeetPosition()
    {
        return _seatPosition.position;
    }
    public Vector3 GetExitPosition()
    {
        return _exitPosition.position;
    }
}
