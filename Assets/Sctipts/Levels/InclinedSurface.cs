using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclinedSurface : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private BoxCollider _trigger;

    public Transform Target => _target;

    public void TriggerDisable()
    {
        _trigger.enabled = false;
    }
}