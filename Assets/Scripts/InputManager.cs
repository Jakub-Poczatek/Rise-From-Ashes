using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
    //, IInputManager
{
    private Action<Vector3> OnMouseRightChangeHandler;
    private Action OnMouseRightUpHandler;
    private Action<Vector3> OnMouseLeftDownHandler;
    private Action OnMouseLeftUpHandler;
    private Action<Vector3> OnMouseChangeHandler;
    private Action<int> OnCameraRotatePerformedHandler;
    private MasterInput masterInput;
    private LayerMask mouseInputMask;

    public LayerMask MouseInputMask 
    {
        get => mouseInputMask;
        set => this.mouseInputMask = value;
    }

    private void Awake()
    {
        masterInput = new();
        masterInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        GetSecondaryInProgress();
    }

    private void GetSecondaryInProgress()
    {
        if (masterInput.Master.Secondary.IsInProgress())
        {
            var position = Input.mousePosition;
            OnMouseRightChangeHandler?.Invoke(position);
        }
    }

    public void OnMouseLeftCallBack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                CallActionOnMouseLeft((position) => OnMouseLeftDownHandler?.Invoke(position));
            CallActionOnMouseLeft((position) => OnMouseChangeHandler?.Invoke(position));
        }
        if (context.canceled)
            OnMouseLeftUpHandler?.Invoke();
    }

    public void OnMouseRightCallBack(InputAction.CallbackContext context)
    {
        if (context.canceled)
            OnMouseRightUpHandler?.Invoke();
    }

    public void OnCameraRotateCallBack(InputAction.CallbackContext context)
    {
        if (context.performed)
            if (context.control.name.Equals("e"))
                OnCameraRotatePerformedHandler.Invoke(-90);
            else if (context.control.name.Equals("q"))
                OnCameraRotatePerformedHandler.Invoke(90);
    }

    private void CallActionOnMouseLeft(Action<Vector3> action)
    {
        Vector3? position = GetMousePosition();
        if (position.HasValue)
        {
            action(position.Value);
            position = null;
        }
    }

    private Vector3? GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3? position = null;
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, Mathf.Infinity, mouseInputMask))
        {
            position = hit.point - transform.position;
        }

        return position;
    }


    public void AddListenerOnMouseLeftDownEvent(Action<Vector3> listener)
    {
        OnMouseLeftDownHandler += listener;
    }

    public void RemoveListenerOnMouseLeftDownEvent(Action<Vector3> listener)
    {
        OnMouseLeftDownHandler -= listener;
    }

    public void AddListenerOnMouseLeftUpEvent(Action listener)
    {
        OnMouseLeftUpHandler += listener;
    }

    public void RemoveListenerOnMouseLeftUpEvent(Action listener)
    {
        OnMouseLeftUpHandler -= listener;
    }

    public void AddListenerOnMouseChangeEvent(Action<Vector3> listener)
    {
        OnMouseChangeHandler += listener;
    }

    public void RemoveListenerOnMouseChangeEvent(Action<Vector3> listener)
    {
        OnMouseChangeHandler -= listener;
    }

    public void AddListenerOnMouseRightChangeEvent(Action<Vector3> listener)
    {
        OnMouseRightChangeHandler += listener;
    }

    public void RemoveListenerOnMouseRightChangeEvent(Action<Vector3> listener)
    {
        OnMouseRightChangeHandler -= listener;
    }

    public void AddListenerOnMouseRightUpEvent(Action listener)
    {
        OnMouseRightUpHandler += listener;
    }

    public void RemoveListenerOnMouseRightUpEvent(Action listener)
    {
        OnMouseRightUpHandler -= listener;
    }

    public void AddListenerOnCameraRotatePerformedEvent(Action<int> listener)
    {
        OnCameraRotatePerformedHandler += listener;
    }
}


/*private void GetPointerPosition()
{
    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
    {
        CallActionOnMouseLeft((position) => OnMouseLeftDownHandler?.Invoke(position));
    }
    if (Input.GetMouseButton(0))
    {
        CallActionOnMouseLeft((position) => OnMouseChangeHandler?.Invoke(position));
    }
    if (Input.GetMouseButtonUp(0))
    {
        OnMouseLeftUpHandler?.Invoke();
    }
}*/
/*private void GetPanningPointer()
{
    if (Input.GetMouseButton(1))
    {
        var position = Input.mousePosition;
        OnMouseRightChangeHandler?.Invoke(position);
    }

    if (Input.GetMouseButtonUp(1))
    {
        OnMouseRightUpHandler?.Invoke();
    }
}*/