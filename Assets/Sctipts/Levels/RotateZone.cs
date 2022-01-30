using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZone : MonoBehaviour
{
    [SerializeField] private Transform _endRotatePosition;
    [SerializeField] private int _rotateDirection;
    [SerializeField] private BoxCollider _collider;

    public Transform EndRotatePosition => _endRotatePosition;
    public int RotateDirection => _rotateDirection;


    public void Disable()
    {
        _collider.enabled = false;
    }
}
