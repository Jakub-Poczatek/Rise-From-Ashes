using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int cellSize = 1;
    public GameObject groundModel;
    private bool buildingModeIsActive = false;
    private GridStructure gridStructure;
    private BuildingManager buildingManager;
    private PlayerState playerState;
    private int width, lenght;
    //public IInputManager inputManager;
    //public InputManager inputManagerConcrete;
    public InputManager inputManager;
    public LayerMask mouseInputMask;
    public CameraMovement cameraMovement;
    public GameObject placementManagerGameObject;
    public UIController uiController;
    public StructureRepository structureRepository;
    public GameObject resourceManagerGameObject;
    private IResourceManager resourceManager;

    public PlayerSelectionState selectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;
    public PlayerBuildingRoadState buildingRoadState;
    public PlayerDemolishingState removeBuildingState;
    public PlayerCitizenAssignState citizenAssignState;

    private IPlacementManager placementManager;


    public PlayerState PlayerState 
    { 
        get => playerState;
    }

    private void Awake()
    {
        /*#if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
        inputManager = gameObject.AddComponent<InputManager>();
        inputManager = inputManagerConcrete;
        #endif*/
        //inputManager = inputManagerConcrete;
        //inputManager = gameObject.AddComponent<InputManager>();
    }

    private void PrepareStates()
    {
        buildingManager = new BuildingManager(placementManager, structureRepository, resourceManager, cellSize, width, lenght);
        resourceManager.PrepareResourceManager(buildingManager);
        selectionState = new PlayerSelectionState(this, buildingManager);
        removeBuildingState = new PlayerDemolishingState(this, buildingManager);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, buildingManager);
        buildingRoadState = new PlayerBuildingRoadState(this, buildingManager);
        citizenAssignState = new PlayerCitizenAssignState(this, buildingManager);
        playerState = selectionState;
    }

    void Start()
    {
        width = (int) groundModel.GetComponent<MeshRenderer>().bounds.size.x;
        lenght = (int) groundModel.GetComponent<MeshRenderer>().bounds.size.z;
        placementManager = placementManagerGameObject.GetComponent<IPlacementManager>();
        resourceManager = resourceManagerGameObject.GetComponent<IResourceManager>();
        PrepareStates();
        PrepareGameComponents();
        AssignInputListener();
        AssignUiControllerListeners();
    }

    private void Update()
    {
    }

    private void PrepareGameComponents()
    {
        inputManager.MouseInputMask = mouseInputMask;
        cameraMovement.SetCameraBounds(0, width, 0, lenght);
    }

    private void AssignUiControllerListeners()
    {
        uiController.AddListenerOnBuildSingleStructureEvent((structureName) => playerState.OnBuildSingleStructure(structureName));
        uiController.AddListenerOnBuildRoadEvent((structureName) => playerState.OnBuildRoad(structureName));
        uiController.AddListenerOnCancelActionEvent(() => playerState.OnCancel());
        uiController.AddListenerOnDemolishActionEvent(() => playerState.OnDemolish());
        uiController.AddListenerOnConfirmActionEvent(() => playerState.OnConfirmAction());
        uiController.AddListenerOnCitizenAssignEvent(() => playerState.OnCitizenAssign());
    }

    private void AssignInputListener()
    {
        inputManager.AddListenerOnMouseLeftDownEvent((position) => playerState.OnInputPointerDown(position));
        inputManager.AddListenerOnMouseRightChangeEvent((position) => playerState.OnInputPanChange(position));
        inputManager.AddListenerOnMouseRightUpEvent(() => playerState.OnInputPanUp());
        inputManager.AddListenerOnMouseChangeEvent((position) => playerState.OnInputPointerChange(position));
        inputManager.AddListenerOnCameraRotatePerformedEvent((angle) => PlayerState.OnCameraRotate(angle));
        inputManager.AddListenerOnCameraZoomPerformedEvent((zoom) => PlayerState.OnCameraZoom(zoom));
    }

    public void TransitionToState(PlayerState newState, string structureName)
    {
        this.playerState = newState;
        this.playerState.EnterState(structureName);
    }
}
