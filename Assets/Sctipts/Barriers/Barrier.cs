using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private Transport _firstTransport;
    [SerializeField] private Transport _secondTransport;
    [SerializeField] private Transform _playerBack;

    private void OnEnable()
    {
        _firstTransport.UsageCancelled += ThrowThePlayerBack;
        _secondTransport.UsageCancelled += ThrowThePlayerBack;
    }

    private void OnDisable()
    {
        _firstTransport.UsageCancelled -= ThrowThePlayerBack;
        _secondTransport.UsageCancelled -= ThrowThePlayerBack;
    }

    private void ThrowThePlayerBack(Player player)
    {
      //  player.ThrowBack(_playerBack.position); 
    }
}