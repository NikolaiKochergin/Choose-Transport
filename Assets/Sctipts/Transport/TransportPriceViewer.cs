using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransportPriceViewer : MonoBehaviour
{
    [SerializeField] private Transport _transport;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private int _moneyWadValue;
    [SerializeField] private float _changeDuration;

    private void OnEnable()
    {
        _priceText.text = _transport.Price.ToString()+"$";
        _transport.StartedUse += OnTransportActivated;
    }

    private void OnDisable()
    {
        _transport.StartedUse -= OnTransportActivated;
    }

    private void OnTransportActivated()
    {
        StartCoroutine(ReducePriceToZero());
    }

    private IEnumerator ReducePriceToZero()
    {
        int tempPrice = _transport.Price;

        float deltaTime = _changeDuration * _moneyWadValue / tempPrice;
        
        while (tempPrice > 0)
        {
            tempPrice -= _moneyWadValue;
            if (tempPrice < 0)
                tempPrice = 0;
            
            _priceText.text = tempPrice + "$";

            yield return new WaitForSeconds(deltaTime);
        }
        gameObject.SetActive(false);
    }
}
