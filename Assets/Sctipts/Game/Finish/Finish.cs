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
        }
    }

    private void OnDisable()
    {
        foreach (var zone in _selectedZones)
        {
            zone.ZoneEntered -= OnZoneSelected;
        }
    }

    private void OnZoneSelected(SelectedZone zone)
    {
        if (_player.Coins >= zone.Price)
        {
            _selectedZone = zone;
            _player.TakeFinish(zone);
            
            if (zone.IsLastZone)
            {
                _player.ScatterRemainingMoney(zone);
                End();
                zone.Select();
            }
            else
            {
                if (_selectedZones[zone.Number + 1].Price > _player.Coins)
                {
                    _player.ScatterRemainingMoney(zone);
                    End();
                    zone.Select();
                }
                else
                {
                    zone.Avoid();
                }
            }
        }
        else if(_selectedZone == null)
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
