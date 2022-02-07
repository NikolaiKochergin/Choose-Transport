using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] private float _sensetivity;

    private float _direction;

    private float _startPosition;
    
    public Input Inputs { get; private set; }
    public float Direction => _direction;

    private void OnEnable()
    {
        Inputs = new Input();
        Inputs.Enable();

        Inputs.MoveLR.TouchDown.started += ctx => SaveStartPosition();
    }

    public float InputDirection()
    {
        return _direction;
    }

    private void Update()
    {
        if (Inputs.MoveLR.TouchDown.IsPressed())
        {
            float currentPosition = Inputs.MoveLR.TouchPos.ReadValue<Vector2>().x;

            float touchDelta = (currentPosition - _startPosition) * _sensetivity / Screen.width;

            _direction = Mathf.Clamp(touchDelta, -1, 1);
        }
        else
        {
            _direction = 0;
        }
    }

    private void SaveStartPosition()
    {
        _startPosition = Inputs.MoveLR.TouchPos.ReadValue<Vector2>().x;
    }
}