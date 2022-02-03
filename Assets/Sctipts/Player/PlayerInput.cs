using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] [Range(0, 2)] private float _sensetivity;

    private float _direction;
    
    public Input Inputs { get; private set; }
    public float Direction => _direction;

    private void OnEnable()
    {
        Inputs = new Input();
        Inputs.Enable();
    }

    public float InputDirection()
    {
        return _direction;
    }

    private void Update()
    {
        float deltaX = Inputs.MoveLR.TouchDelta.ReadValue<float>();

        if (Inputs.MoveLR.TouchDown.IsPressed())
        {
            _direction = Mathf.Clamp(deltaX * _sensetivity, -1, 1);
        }
        else
        {
            _direction = 0;
        }
    }
}