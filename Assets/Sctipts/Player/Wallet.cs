using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wallet : MonoBehaviour
{
    [SerializeField] private Player _player;

    private int _coins;

    public int Coins => _coins;

    public event UnityAction<int> CoinsChanged;

    private void OnEnable()
    {
        _coins = 0;

        _player.CollideWithMoney += OnPlayerCollideWithMoney;
        _player.CollideWithBarrier += OnPlayerCollideWithMoneyBarrier;
        _player.TransportPurchased += OnTransportPurchased;
        _player.FinishZoneAvoided += OnPlayerCollideWithMoneyBarrier;
    }

    private void OnDisable()
    {
        _player.CollideWithMoney -= OnPlayerCollideWithMoney;
        _player.CollideWithBarrier -= OnPlayerCollideWithMoneyBarrier;
        _player.TransportPurchased -= OnTransportPurchased;
        _player.FinishZoneAvoided -= OnPlayerCollideWithMoneyBarrier;
    }

    private void OnPlayerCollideWithMoney(int coins)
    {
        if (coins > 0)
        {
            _coins += coins;
            CoinsChanged?.Invoke(_coins);
        }
    }

    private void OnPlayerCollideWithMoneyBarrier(int coins)
    {
        if (_coins - coins < 0)
            _coins = 0;
        else
            _coins -= coins;

        CoinsChanged?.Invoke(_coins);
    }

    private void OnTransportPurchased(int price)
    {
        _coins -= price;

        CoinsChanged?.Invoke(_coins);
    }
}