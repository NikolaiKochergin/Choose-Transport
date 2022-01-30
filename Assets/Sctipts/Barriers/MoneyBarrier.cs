using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBarrier : MonoBehaviour
{
    [SerializeField] private int _coinsCrash;
    [SerializeField] private int _hitJumpDirection;
    [SerializeField] private bool _canDestroy;
    [SerializeField] private bool _hitJump;
    [SerializeField] private bool _explosionDestroy;
    [SerializeField] private Rigidbody[] _rigibodyies;
    [SerializeField] private Transform _explostionPosition;
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private float _explosionForce;
    public int CoinsCrush => _coinsCrash;
    public int HitJumpDirection => _hitJumpDirection;

    public bool HitJump => _hitJump;

    private bool _isActive = false;

    public void CollideWithPlayer()
    {
        if (!_isActive)
        {
            _isActive = true;

            _hitEffect.gameObject.SetActive(true);
            _hitEffect.Play();


            if (_explosionDestroy)
                Explosion();

            if (_canDestroy)
                Destroy(gameObject);
        }
    }


    private void Explosion()
    {
        foreach(var rigidbody in _rigibodyies)
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
            rigidbody.AddExplosionForce(_explosionForce, _explostionPosition.position,6);
        }
    }
}
