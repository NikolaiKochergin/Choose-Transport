using UnityEngine;

public class MoneyTextFollower : MonoBehaviour
{
    private Camera _camera;
    
    private void Awake()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        transform.LookAt(_camera.transform.position);
    }
}
