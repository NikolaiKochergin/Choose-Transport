using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHitBackZone : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffect;

    public void Hit()
    {
        _hitEffect.gameObject.SetActive(true);
        _hitEffect.Play();
    }
}
