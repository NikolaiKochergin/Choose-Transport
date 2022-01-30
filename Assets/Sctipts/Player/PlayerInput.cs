using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float _minPressedTime;

    private float _direction = 0;
    private Vector2 _lastPosition;
    private float _lastPressedTime;
    public Input Inputs { get; private set; }

    public bool IsToched { get; private set; }
    public float Direction => _direction;

    private void OnEnable()
    {
        Inputs = new Input();

        Inputs.MoveLR.TouchDown.started += TouchDown_started;
        Inputs.MoveLR.TouchDown.canceled += TouchDown_canceled;

        EnebleInput();
    }

    private Vector2 _touchPosStart;

    private void TouchDown_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _touchPosStart = GetTouchPos();
        _lastPosition = GetTouchPos();
        IsToched = true;
        _lastPressedTime = Time.time;
    }

    private void TouchDown_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        IsToched = false;
        Vector2 _touchPosCancel = GetTouchPos();
        Vector2 vecDis = (_touchPosCancel - _touchPosStart).normalized;
        float len = Vector2.Distance(_touchPosCancel, _touchPosStart);
        _direction = 0;
        _lastPressedTime = Time.time;
    }

    public float InputDirection()
    {
        if (!IsToched)
            return 0;
        else
        {
            _direction = 0;

            Vector2 delta = (GetTouchPos() - _lastPosition);
            if (delta.x > 3.5f | delta.x < -3.5f)
                _direction = delta.normalized.x;

            _lastPosition = GetTouchPos();

            return _direction;
        }
    }
    

    public Vector2 GetTouchPos()
    {
        return Inputs.MoveLR.TouchPos.ReadValue<Vector2>();
    }

    public void EnebleInput()
    {
        Inputs.Enable();
    }

    public void DisebleInput()
    {
        Inputs.Disable();
    }
}