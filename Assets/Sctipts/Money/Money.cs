using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private int _coins;
    [SerializeField] private ParticleSystem _moneyEffect;
    [SerializeField] private GameObject _moneyObject;

    public int Coins => _coins;

    public void CollideWithPlayer()
    {
        GetComponent<Collider>().enabled = false;
        _moneyEffect.gameObject.SetActive(true);
        _moneyEffect.Play();
        _moneyObject.gameObject.SetActive(false);
    }

}