using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] private Transform _seatPosition;

    public Vector3 SeatPosition => _seatPosition.position;
}
