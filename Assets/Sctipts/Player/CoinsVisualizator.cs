using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsVisualizator : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private Player _player;
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _moneyContainer;
    [SerializeField] private MoneyWad _moneyWadPrefab;
    [SerializeField] private float _offsetY;

    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform[] _placePoints;
    [SerializeField] private int _moneyForChangeBag;
    [SerializeField] private float _receiveMoneyDuration;
    [SerializeField] private float _spendMoneyDuration;

    private List<MoneyWad> _spawnedMoneyWads;

    private Coroutine _addMoney;
    private Coroutine _removeMoney;

    private void Awake()
    {
        _spawnedMoneyWads = new List<MoneyWad>();
    }

    private void OnEnable()
    {
        _wallet.CoinsChanged += CoinsChanged;

        _player.StartedRun += ShowCoins;
        _player.EndUseTransport += ActivateText;
        _player.StartedUseTransport += StartUseTransport;
        _player.StartedFinishedMove += DisableMoneyContainer;
        _player.Failed += DisableText;

        _player.StartedExitFromTransport += DisableMoneyContainer;
        _player.EndExitFromTransport += ActivateText;
    }


    private void OnDisable()
    {
        _wallet.CoinsChanged -= CoinsChanged;
        
        _player.StartedRun -= ShowCoins;
        _player.StartedFinishedMove -= DisableMoneyContainer;
        _player.EndUseTransport -= ActivateText;
        _player.StartedUseTransport -= StartUseTransport;
        _player.Failed -= DisableText;

        _player.StartedExitFromTransport -= DisableMoneyContainer;
        _player.EndExitFromTransport -= ActivateText;
    }

    private void ShowCoins()
    {
        _coinsText.text = _wallet.Coins + "$";
    }

    private void CoinsChanged(int currentCoins, Transform disappearPoint)
    {
        _coinsText.text = currentCoins + "$";

        int moneyWadCount = currentCoins / _moneyForChangeBag;

        if (moneyWadCount > _spawnedMoneyWads.Count)
        {
            AddMoneyWadOnContainer(moneyWadCount);
        }
        else if (moneyWadCount < _spawnedMoneyWads.Count)
        {
            RemoveMoneyWadOnContainer(moneyWadCount, disappearPoint);
        }
    }

    private void DisableMoneyContainer()
    {
        _moneyContainer.SetActive(false);
    }

    private void DisableText()
    {
        _moneyContainer.SetActive(false);
        _coinsText.gameObject.SetActive(false);
    }

    private void StartUseTransport(string text)
    {
        _animator.Play("DisappearAnimation");
        _coinsText.gameObject.SetActive(true);
    }

    private void ActivateText()
    {
        _moneyContainer.SetActive(true);
        _animator.Play("AppearAnimation");
        _coinsText.gameObject.SetActive(true);
    }

    private void AddMoneyWadOnContainer(int moneyWadCount)
    {
        if (_removeMoney != null)
            StopCoroutine(_removeMoney);
        if (_addMoney != null)
            StopCoroutine(_addMoney);
        _addMoney = StartCoroutine(AddMoneyWadTo(moneyWadCount));
    }

    private void RemoveMoneyWadOnContainer(int moneyWadCount, Transform disappearPoint)
    {
        if (_addMoney != null)
            StopCoroutine(_addMoney);
        if (_removeMoney != null)
            StopCoroutine(_removeMoney);
        _removeMoney = StartCoroutine(RemoveMoneyWadTo(moneyWadCount, disappearPoint));
    }

    private void SpawnMoneyWad(Vector3 position)
    {
        MoneyWad moneyWad = Instantiate(_moneyWadPrefab, _moneyContainer.transform);
        moneyWad.transform.localPosition = position;
        moneyWad.ShowAppear(_spawnPoint);
        _spawnedMoneyWads.Add(moneyWad);
    }

    private void RemoveMoneyWad(MoneyWad moneyWad, Transform disappearPoint)
    {
        _spawnedMoneyWads.Remove(moneyWad);
        moneyWad.ShowDisappear(disappearPoint);
    }

    private IEnumerator AddMoneyWadTo(int count)
    {
        float deltaTime = _receiveMoneyDuration / (count - _spawnedMoneyWads.Count);
        
        for (int i = _spawnedMoneyWads.Count; i < count; i++)
        {
            int index = _spawnedMoneyWads.Count % _placePoints.Length;

            SpawnMoneyWad(_placePoints[index].transform.localPosition +
                          Vector3.up * _offsetY * (i / _placePoints.Length));

            yield return new WaitForSeconds(deltaTime);
        }
    }

    private IEnumerator RemoveMoneyWadTo(int count, Transform disappearPoint)
    {
        float deltaTime = _spendMoneyDuration / (_spawnedMoneyWads.Count - count);
        
        for (int i = _spawnedMoneyWads.Count; i > count; i--)
        {
            RemoveMoneyWad(_spawnedMoneyWads[i - 1], disappearPoint);
            yield return new WaitForSeconds(deltaTime);
        }
    }
}