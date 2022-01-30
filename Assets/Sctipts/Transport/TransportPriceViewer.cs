using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransportPriceViewer : MonoBehaviour
{
    [SerializeField] private Transport _transport;
    [SerializeField] private TMP_Text _priceText;

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
        gameObject.SetActive(false);
    }
}
