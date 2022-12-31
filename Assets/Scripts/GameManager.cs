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
    public StructureRepository structureRepository;

    public PlayerSelectionState playerSelectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;
    public PlayerBuildingRoadState buildingRoadState;
    public PlayerRemoveBuildingState playerRemoveBuildingState;

    public PlayerState PlayerState 
    { 
        get => playerState;
    }

    private void Awake()
    {
        PrepareStates();


        #if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
                inputManager = gameObject.AddComponent<InputManager>();
        #endif
    }

    private void PrepareStates()
    {
        buildingManager = new BuildingManager(placementManager, resourceManager, structureRepository, cellSize, width, length);
        playerSelectionState = new PlayerSelectionState(this, cameraMovement);
        playerRemoveBuildingState = new PlayerRemoveBuildingState(this, buildingManager);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, buildingManager);
        buildingRoadState = new PlayerBuildingRoadState(this, buildingManager);
        playerState = playerSelectionState;
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
        uiController.AddListenerOnBuildSingleStructureEvent((structureName) => playerState.OnBuildSingleStructure(structureName));
        uiController.AddListenerOnBuildRoadEvent((structureName) => playerState.OnBuildRoad(structureName));
        //uiController.AddListenerOnBuildAreaEvent(ChangeToBuildingSingleStructureState);
        uiController.AddListenerOnCancelActionEvent(() => playerState.OnCancel());
        uiController.AddListenerOnDemolishActionEvent(() => playerState.OnDemolish());
    }

    private void AssignInputListener()
    {
        inputManager.AddListenerOnPointerDownEvent((position) => playerState.OnInputPointerDown(position));
        inputManager.AddListenerOnPointerSecondChangeEvent((position) => playerState.OnInputPanChange(position));
        inputManager.AddListenerOnPointerSecondUpEvent(() => playerState.OnInputPanUp());
        inputManager.AddListenerOnPointerChangeEvent((position) => playerState.OnInputPointerChange(position));
    }

    public void TransitionToState(PlayerState newState, string structureName)
    {
        this.playerState = newState;
        this.playerState.EnterState(structureName);
    }
}
