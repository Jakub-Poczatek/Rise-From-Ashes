using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
    //, IInputManager
{
    private Action<Vector3> OnPointerSecondChangeHandler;
    private Action OnPointerSecondUpHandler;
    private Action<Vector3> OnPointerDownHandler;
    private Action OnPointerUpHandler;
    private Action<Vector3> OnPointerChangeHandler;
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
            OnPointerSecondChangeHandler?.Invoke(position);
        }
    }

    public void OnMouseLeftDownCallBack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                CallActionOnPointer((position) => OnPointerDownHandler?.Invoke(position));
            CallActionOnPointer((position) => OnPointerChangeHandler?.Invoke(position));
        }
        if (context.canceled)
            OnPointerUpHandler?.Invoke();
    }

    public void OnMouseRightCallBack(InputAction.CallbackContext context)
    {
        if (context.canceled)
            OnPointerSecondUpHandler?.Invoke();
    }

    private void CallActionOnPointer(Action<Vector3> action)
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


    public void AddListenerOnPointerDownEvent(Action<Vector3> listener)
    {
        OnPointerDownHandler += listener;
    }

    public void RemoveListenerOnPointerDownEvent(Action<Vector3> listener)
    {
        OnPointerDownHandler -= listener;
    }

    public void AddListenerOnPointerUpEvent(Action listener)
    {
        OnPointerUpHandler += listener;
    }

    public void RemoveListenerOnPointerUpEvent(Action listener)
    {
        OnPointerUpHandler -= listener;
    }

    public void AddListenerOnPointerChangeEvent(Action<Vector3> listener)
    {
        OnPointerChangeHandler += listener;
    }

    public void RemoveListenerOnPointerChangeEvent(Action<Vector3> listener)
    {
        OnPointerChangeHandler -= listener;
    }

    public void AddListenerOnPointerSecondChangeEvent(Action<Vector3> listener)
    {
        OnPointerSecondChangeHandler += listener;
    }

    public void RemoveListenerOnPointerSecondChangeEvent(Action<Vector3> listener)
    {
        OnPointerSecondChangeHandler -= listener;
    }

    public void AddListenerOnPointerSecondUpEvent(Action listener)
    {
        OnPointerSecondUpHandler += listener;
    }

    public void RemoveListenerOnPointerSecondUpEvent(Action listener)
    {
        OnPointerSecondUpHandler -= listener;
    }
}


/*private void GetPointerPosition()
{
    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
    {
        CallActionOnPointer((position) => OnPointerDownHandler?.Invoke(position));
    }
    if (Input.GetMouseButton(0))
    {
        CallActionOnPointer((position) => OnPointerChangeHandler?.Invoke(position));
    }
    if (Input.GetMouseButtonUp(0))
    {
        OnPointerUpHandler?.Invoke();
    }
}*/
/*private void GetPanningPointer()
{
    if (Input.GetMouseButton(1))
    {
        var position = Input.mousePosition;
        OnPointerSecondChangeHandler?.Invoke(position);
    }

    if (Input.GetMouseButtonUp(1))
    {
        OnPointerSecondUpHandler?.Invoke();
    }
}*/