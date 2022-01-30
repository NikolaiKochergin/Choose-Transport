using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TransportTrigger : MonoBehaviour
{
    public event UnityAction<Player> TransportSelected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (!other.GetComponent<Player>().IsUseTransport)
            {
                TransportSelected?.Invoke(other.GetComponent<Player>());
                gameObject.SetActive(false);
            }
        }
    }
}