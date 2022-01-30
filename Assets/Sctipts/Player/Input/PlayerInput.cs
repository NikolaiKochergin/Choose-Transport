// GENERATED AUTOMATICALLY FROM 'Assets/Sctipts/Player/Input/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Input : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Input()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""MoveLR"",
            ""id"": ""152ed535-ef29-45cb-ac0a-7954e2d4352d"",
            ""actions"": [
                {
                    ""name"": ""TouchPos"",
                    ""type"": ""Value"",
                    ""id"": ""92b3e33a-5750-488c-a6b5-fcf911ab563a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchDown"",
                    ""type"": ""Button"",
                    ""id"": ""6a63a335-f524-454b-8912-564e9511239c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5cd386c0-1359-4bb9-93e1-d85f2ad6e329"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""TouchPos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa228017-f3bc-4cde-9d0c-d9dc1a655f62"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""TouchDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player"",
            ""bindingGroup"": ""Player"",
            ""devices"": []
        }
    ]
}");
        // MoveLR
        m_MoveLR = asset.FindActionMap("MoveLR", throwIfNotFound: true);
        m_MoveLR_TouchPos = m_MoveLR.FindAction("TouchPos", throwIfNotFound: true);
        m_MoveLR_TouchDown = m_MoveLR.FindAction("TouchDown", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // MoveLR
    private readonly InputActionMap m_MoveLR;
    private IMoveLRActions m_MoveLRActionsCallbackInterface;
    private readonly InputAction m_MoveLR_TouchPos;
    private readonly InputAction m_MoveLR_TouchDown;
    public struct MoveLRActions
    {
        private @Input m_Wrapper;
        public MoveLRActions(@Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @TouchPos => m_Wrapper.m_MoveLR_TouchPos;
        public InputAction @TouchDown => m_Wrapper.m_MoveLR_TouchDown;
        public InputActionMap Get() { return m_Wrapper.m_MoveLR; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MoveLRActions set) { return set.Get(); }
        public void SetCallbacks(IMoveLRActions instance)
        {
            if (m_Wrapper.m_MoveLRActionsCallbackInterface != null)
            {
                @TouchPos.started -= m_Wrapper.m_MoveLRActionsCallbackInterface.OnTouchPos;
                @TouchPos.performed -= m_Wrapper.m_MoveLRActionsCallbackInterface.OnTouchPos;
                @TouchPos.canceled -= m_Wrapper.m_MoveLRActionsCallbackInterface.OnTouchPos;
                @TouchDown.started -= m_Wrapper.m_MoveLRActionsCallbackInterface.OnTouchDown;
                @TouchDown.performed -= m_Wrapper.m_MoveLRActionsCallbackInterface.OnTouchDown;
                @TouchDown.canceled -= m_Wrapper.m_MoveLRActionsCallbackInterface.OnTouchDown;
            }
            m_Wrapper.m_MoveLRActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TouchPos.started += instance.OnTouchPos;
                @TouchPos.performed += instance.OnTouchPos;
                @TouchPos.canceled += instance.OnTouchPos;
                @TouchDown.started += instance.OnTouchDown;
                @TouchDown.performed += instance.OnTouchDown;
                @TouchDown.canceled += instance.OnTouchDown;
            }
        }
    }
    public MoveLRActions @MoveLR => new MoveLRActions(this);
    private int m_PlayerSchemeIndex = -1;
    public InputControlScheme PlayerScheme
    {
        get
        {
            if (m_PlayerSchemeIndex == -1) m_PlayerSchemeIndex = asset.FindControlSchemeIndex("Player");
            return asset.controlSchemes[m_PlayerSchemeIndex];
        }
    }
    public interface IMoveLRActions
    {
        void OnTouchPos(InputAction.CallbackContext context);
        void OnTouchDown(InputAction.CallbackContext context);
    }
}
