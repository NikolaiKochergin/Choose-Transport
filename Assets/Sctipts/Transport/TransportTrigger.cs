using UnityEngine;
using UnityEngine.Events;

public class TransportTrigger : MonoBehaviour
{
    public event UnityAction<Player> TransportSelected;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();

        if (player)
        {
            if (player.IsUseTransport == false)
            {
                TransportSelected?.Invoke(other.GetComponent<Player>());
            }
        }
    }
}