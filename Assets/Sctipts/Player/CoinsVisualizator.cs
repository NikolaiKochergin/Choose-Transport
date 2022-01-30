using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsVisualizator : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private Player _player;

   // [SerializeField] private GameObject _moneyContainer;
    [SerializeField] private GameObject _moneyPrefab;

    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private int _moneyForChangeBag;
    [SerializeField] private float _speedChangeMoney;

    private List<GameObject> _spawnedMoney = new List<GameObject>();
    private int _currentNumber=0;
    private int _spawnedMoneyCount=0;
    private int _currentConis=0;

    private IEnumerator _addMoney;
    private IEnumerator _removeMoney;

    private void OnEnable()
    {
        //_currentConis = _wallet.Coins;

        _wallet.CoinsChanged += CoinsChanged;

        _player.StartedRun += ShowCoins;
        _player.EndUseTransport += ActivateText;
        _player.StartedUseTransport += StartUseTransport;
        _player.StartedFinishedMove += DisableText;
        _player.Failed += DisableText;

        _player.StartedExitFromTransport += DisableText;
        _player.EndExitFromTransport += ActivateText;
    }


    private void OnDisable()
    {
        _wallet.CoinsChanged -= CoinsChanged;
        _player.StartedRun -= ShowCoins;
        _player.StartedFinishedMove -= DisableText;
        _player.EndUseTransport -= ActivateText;
        _player.StartedUseTransport -= StartUseTransport;
        _player.Failed -= DisableText;

        _player.StartedExitFromTransport -= DisableText;
        _player.EndExitFromTransport -= ActivateText;
    }

    private void ShowCoins()
    {
        CoinsChanged(_wallet.Coins);
    }

    private void CoinsChanged(int currentCoins)
    {
        _coinsText.text = currentCoins.ToString()+"$";

        if (currentCoins > _currentConis )
        {
            int moneyCount = (currentCoins - _currentConis) / _moneyForChangeBag;
            if (moneyCount >= 1)
            {
               // AddMoneyOnContainer(moneyCount);
                _currentConis = currentCoins;
            }
        }
        else if( _currentConis > currentCoins)
        {
            int moneyCount = (_currentConis - currentCoins) / _moneyForChangeBag;

            if (moneyCount >= 1)
            {
              //  RemoveMoneyOnContainer(moneyCount);
                _currentConis = currentCoins;
            }
        }
    }

    private void DisableText()
    {
    //    _moneyContainer.SetActive(false);
        _coinsText.gameObject.SetActive(false);
    }
    private void StartUseTransport(string text)
    {
  //      _moneyContainer.SetActive(false);
        _coinsText.gameObject.SetActive(true);
    }

    private void ActivateText()
    {
   //     _moneyContainer.SetActive(true);
        _coinsText.gameObject.SetActive(true);
    }

    private void AddMoneyOnContainer(int moneyCount)
    {
        if(_addMoney!=null)
            StopCoroutine(_addMoney);
      //  _addMoney = AddMoney(moneyCount);
      //  StartCoroutine(_addMoney);
    }

    private void RemoveMoneyOnContainer(int moneyCount)
    {
        if (_removeMoney != null)
            StopCoroutine(_removeMoney);

        _removeMoney = RemoveMoney(moneyCount);
        StartCoroutine(_removeMoney);
    }

    private void SpawnMoney(Vector3 position)
    {
      //  var money = Instantiate(_moneyPrefab, _moneyContainer.transform);
     //   money.transform.position = position;
    //    _spawnedMoney.Add(money);
    }

    private IEnumerator AddMoney(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_spawnedMoneyCount <= _spawnPoints.Length - 1)
            {
                SpawnMoney(_spawnPoints[_currentNumber].position);
                _currentNumber++;
                _spawnedMoneyCount++;
            }
            yield return new WaitForSeconds(_speedChangeMoney);
        }
    }

    private IEnumerator RemoveMoney(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_spawnedMoneyCount >= 0)
            {
                _spawnedMoney[_spawnedMoney.Count - 1].gameObject.SetActive(false);
                _spawnedMoneyCount--;
                _currentNumber--;
            }
            yield return new WaitForSeconds(_speedChangeMoney);
                
        }
    }

}