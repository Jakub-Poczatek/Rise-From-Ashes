//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.0
//     from Assets/Input/MasterInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @MasterInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MasterInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MasterInput"",
    ""maps"": [
        {
            ""name"": ""Master"",
            ""id"": ""c0c9ab8a-545c-401f-aec7-989004c66467"",
            ""actions"": [
                {
                    ""name"": ""Primary"",
                    ""type"": ""Button"",
                    ""id"": ""d61e7fe3-7566-4697-a02b-fb4a4975074e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Secondary"",
                    ""type"": ""Button"",
                    ""id"": ""46760f74-ddc1-4085-b316-ec444004de76"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CameraRotate"",
                    ""type"": ""Button"",
                    ""id"": ""0501b522-63dd-4ddf-bf69-a65017f170bb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CameraZoom"",
                    ""type"": ""Value"",
                    ""id"": ""809952e6-5892-48f0-8ebd-e48c2cd939cd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CameraMove"",
                    ""type"": ""Value"",
                    ""id"": ""2b113205-f973-4a8e-b492-8cf096796821"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c26e6676-35c9-4119-8023-0bc772b6d388"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Primary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""46d34bea-66bb-4e60-bc35-b2b6abab810c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Secondary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99ad0ef5-1d29-46f6-87e5-6e822830a40c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CameraRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d00a42a5-53c6-4702-83b5-77b934800f77"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CameraRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""06c4e754-53c7-4ec8-a12b-bf43e7590792"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CameraZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Direction"",
                    ""id"": ""f4fbb398-e176-4fe5-a1b5-252aee45a702"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""33cebb5c-c77a-489d-bb89-ad7397542d8d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CameraMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fe236835-5445-410f-9bbc-0852f850a21f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CameraMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""967d3bef-9c8a-4d63-8647-08a4e11b7b54"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CameraMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3c430c8c-1b41-499f-a5dd-1edc1ffbc99e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CameraMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Master
        m_Master = asset.FindActionMap("Master", throwIfNotFound: true);
        m_Master_Primary = m_Master.FindAction("Primary", throwIfNotFound: true);
        m_Master_Secondary = m_Master.FindAction("Secondary", throwIfNotFound: true);
        m_Master_CameraRotate = m_Master.FindAction("CameraRotate", throwIfNotFound: true);
        m_Master_CameraZoom = m_Master.FindAction("CameraZoom", throwIfNotFound: true);
        m_Master_CameraMove = m_Master.FindAction("CameraMove", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Master
    private readonly InputActionMap m_Master;
    private List<IMasterActions> m_MasterActionsCallbackInterfaces = new List<IMasterActions>();
    private readonly InputAction m_Master_Primary;
    private readonly InputAction m_Master_Secondary;
    private readonly InputAction m_Master_CameraRotate;
    private readonly InputAction m_Master_CameraZoom;
    private readonly InputAction m_Master_CameraMove;
    public struct MasterActions
    {
        private @MasterInput m_Wrapper;
        public MasterActions(@MasterInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Primary => m_Wrapper.m_Master_Primary;
        public InputAction @Secondary => m_Wrapper.m_Master_Secondary;
        public InputAction @CameraRotate => m_Wrapper.m_Master_CameraRotate;
        public InputAction @CameraZoom => m_Wrapper.m_Master_CameraZoom;
        public InputAction @CameraMove => m_Wrapper.m_Master_CameraMove;
        public InputActionMap Get() { return m_Wrapper.m_Master; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MasterActions set) { return set.Get(); }
        public void AddCallbacks(IMasterActions instance)
        {
            if (instance == null || m_Wrapper.m_MasterActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MasterActionsCallbackInterfaces.Add(instance);
            @Primary.started += instance.OnPrimary;
            @Primary.performed += instance.OnPrimary;
            @Primary.canceled += instance.OnPrimary;
            @Secondary.started += instance.OnSecondary;
            @Secondary.performed += instance.OnSecondary;
            @Secondary.canceled += instance.OnSecondary;
            @CameraRotate.started += instance.OnCameraRotate;
            @CameraRotate.performed += instance.OnCameraRotate;
            @CameraRotate.canceled += instance.OnCameraRotate;
            @CameraZoom.started += instance.OnCameraZoom;
            @CameraZoom.performed += instance.OnCameraZoom;
            @CameraZoom.canceled += instance.OnCameraZoom;
            @CameraMove.started += instance.OnCameraMove;
            @CameraMove.performed += instance.OnCameraMove;
            @CameraMove.canceled += instance.OnCameraMove;
        }

        private void UnregisterCallbacks(IMasterActions instance)
        {
            @Primary.started -= instance.OnPrimary;
            @Primary.performed -= instance.OnPrimary;
            @Primary.canceled -= instance.OnPrimary;
            @Secondary.started -= instance.OnSecondary;
            @Secondary.performed -= instance.OnSecondary;
            @Secondary.canceled -= instance.OnSecondary;
            @CameraRotate.started -= instance.OnCameraRotate;
            @CameraRotate.performed -= instance.OnCameraRotate;
            @CameraRotate.canceled -= instance.OnCameraRotate;
            @CameraZoom.started -= instance.OnCameraZoom;
            @CameraZoom.performed -= instance.OnCameraZoom;
            @CameraZoom.canceled -= instance.OnCameraZoom;
            @CameraMove.started -= instance.OnCameraMove;
            @CameraMove.performed -= instance.OnCameraMove;
            @CameraMove.canceled -= instance.OnCameraMove;
        }

        public void RemoveCallbacks(IMasterActions instance)
        {
            if (m_Wrapper.m_MasterActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMasterActions instance)
        {
            foreach (var item in m_Wrapper.m_MasterActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MasterActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MasterActions @Master => new MasterActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IMasterActions
    {
        void OnPrimary(InputAction.CallbackContext context);
        void OnSecondary(InputAction.CallbackContext context);
        void OnCameraRotate(InputAction.CallbackContext context);
        void OnCameraZoom(InputAction.CallbackContext context);
        void OnCameraMove(InputAction.CallbackContext context);
    }
}
