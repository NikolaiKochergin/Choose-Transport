using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SelectedZone : MonoBehaviour
{
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private bool _isLastZone;
    [SerializeField] private int _price;
    [SerializeField] private ParticleSystem _selectEffect;
    [SerializeField] private ParticleSystem _avoidEffect;
    [SerializeField] private int _number;
    [SerializeField] private Transform _playerMovementTarget;
    [SerializeField] private Transform _cameraTarget;

    private float _delayBeforeStop = 0.5f;
    public event UnityAction<SelectedZone> ZoneEntered;
    public int Price => _price;
    public int Number => _number;
    public float DelayBeforeStop => _delayBeforeStop;

    public bool IsLastZone => _isLastZone;
    public Transform PlayerMovementTarget => _playerMovementTarget;
    public Transform CameraTarget => _cameraTarget;


    private void OnEnable()
    {
        _priceText.text = _price.ToString() + "$";
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        
        if (player)
        {
            if (_isLastZone)
                StartCoroutine(StopWithDelay());
            else
                StartCoroutine(StopWithDelay());
        }
    }

    public void Avoid()
    {
        _priceText.enabled = false;
        _avoidEffect.gameObject.SetActive(true);
        _avoidEffect.Play();
    }

    public void Select()
    {
        Avoid();
        _selectEffect.gameObject.SetActive(true);
        _selectEffect.Play();
    }

    private IEnumerator StopWithDelay()
    {
        yield return new WaitForSeconds(_delayBeforeStop);

        ZoneEntered?.Invoke(this);
    }
}