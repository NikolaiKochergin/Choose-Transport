using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Finish : MonoBehaviour
{
    [SerializeField] private SelectedZone[] _selectedZones;
    [SerializeField] private Player _player;

    private SelectedZone _selectedZone;

    public event UnityAction<Transform> ZoneSelected;

    private void OnEnable()
    {
        foreach(var zone in _selectedZones)
        {
            zone.ZoneEntered += OnZoneSelected;
            zone.LastZoneEntered += End;
        }
    }

    private void OnDisable()
    {
        foreach (var zone in _selectedZones)
        {
            zone.ZoneEntered -= OnZoneSelected;
            zone.LastZoneEntered -= End;
        }
    }


    private void OnZoneSelected(SelectedZone zone)
    {
        if (_player.Coins >= zone.Price)
        {
            _selectedZone = zone;
            if (!zone.IsLastZone)
            {
                if (_selectedZones[zone.Number + 1].Price > _player.Coins)
                {
                    End();
                    zone.Select();
                }
                else
                    zone.Avoid();
            }
            else
            {
                End();
                zone.Select();
            }
        }
        else
        {
            _player.FailedOnFinish();
        }
    }


    private void End()
    {
        _player.EnterOnFinishZone(_selectedZone);
        ZoneSelected?.Invoke(_selectedZone.CameraTarget);
    }
}
