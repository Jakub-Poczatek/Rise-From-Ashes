using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int cellSize = 1;
    private bool buildingModeIsActive = false;
    private GridStructure gridStructure;
    private PlayerState playerState;
    public int width, length;
    public IInputManager inputManager;
    public CameraMovement cameraMovement;
    public PlacementManager placementManager;
    public UIController uiController;
    public PlayerSelectionState playerSelectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;

    public PlayerState PlayerState 
    { 
        get => playerState;
    }

    private void Awake()
    {
        gridStructure = new GridStructure(cellSize, width, length);
        playerSelectionState = new PlayerSelectionState(this, cameraMovement);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, placementManager, gridStructure);
        playerState = playerSelectionState;
    }

    void Start()
    {
        cameraMovement.SetCameraBounds(0, width, 0, length);
        inputManager = FindObjectsOfType<MonoBehaviour>().OfType<IInputManager>().FirstOrDefault();
        
        inputManager.AddListenerOnPointerDownEvent(HandleInput);
        inputManager.AddListenerOnPointerSecondChangeEvent(HandleInputCameraPan);
        inputManager.AddListenerOnPointerSecondUpEvent(HandleInptuCameraPanStop);
        inputManager.AddListenerOnPointerChangeEvent(HandlePointerChange);
        uiController.AddListenerOnBuildAreaEvent(ChangeToBuildingSingleStructureState);
        uiController.AddListenerOnCancelActionEvent(CancelState);
    }

    private void HandlePointerChange(Vector3 position)
    {
        playerState.OnInputPointerChange(position);
    }

    private void HandleInptuCameraPanStop()
    {
        playerState.OnInputPanUp();
    }

    private void HandleInputCameraPan(Vector3 position)
    {
        playerState.OnInputPanChange(position);
    }

    private void HandleInput(Vector3 position)
    {
        playerState.OnInputPointerDown(position);
    }

    private void ChangeToBuildingSingleStructureState()
    {
        TransitionToState(buildingSingleStructureState);
    }

    private void CancelState()
    {
        playerState.OnCancel();
    }

    public void TransitionToState(PlayerState newState)
    {
        this.playerState = newState;
        this.playerState.EnterState();
    }
}
