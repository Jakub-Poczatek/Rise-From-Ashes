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
    private BuildingManager buildingManager;
    private PlayerState playerState;
    public int width, length;
    public IInputManager inputManager;
    public LayerMask mouseInputMask;
    public CameraMovement cameraMovement;
    public PlacementManager placementManager;
    public UIController uiController;
    public ResourceManager resourceManager;
    public PlayerSelectionState playerSelectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;
    public PlayerRemoveBuildingState playerRemoveBuildingState;

    public PlayerState PlayerState 
    { 
        get => playerState;
    }

    private void Awake()
    {
        buildingManager = new BuildingManager(placementManager, resourceManager, cellSize, width, length);
        playerSelectionState = new PlayerSelectionState(this, cameraMovement);
        playerRemoveBuildingState = new PlayerRemoveBuildingState(this, buildingManager);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, buildingManager);
        playerState = playerSelectionState;


        #if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
        inputManager = gameObject.AddComponent<InputManager>();
        #endif
    }

    void Start()
    {
        PrepareGameComponents();
        AssignInputListener();
        AssignUiControllerListeners();
    }

    private void Update()
    {
        uiController.goldAmountTxt.text = resourceManager.GoldAmount.ToString();
    }

    private void PrepareGameComponents()
    {
        inputManager.MouseInputMask = mouseInputMask;
        cameraMovement.SetCameraBounds(0, width, 0, length);
    }

    private void AssignUiControllerListeners()
    {
        uiController.AddListenerOnBuildAreaEvent(ChangeToBuildingSingleStructureState);
        uiController.AddListenerOnCancelActionEvent(CancelState);
        uiController.AddListenerOnDemolishActionEvent(ChangeToRemoveBuildingState);
    }

    private void AssignInputListener()
    {
        inputManager.AddListenerOnPointerDownEvent(HandleInput);
        inputManager.AddListenerOnPointerSecondChangeEvent(HandleInputCameraPan);
        inputManager.AddListenerOnPointerSecondUpEvent(HandleInptuCameraPanStop);
        inputManager.AddListenerOnPointerChangeEvent(HandlePointerChange);
    }

    private void ChangeToRemoveBuildingState()
    {
        TransitionToState(playerRemoveBuildingState);
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
