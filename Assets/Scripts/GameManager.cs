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
    public IInputManager inputManager;
    public LayerMask mouseInputMask;
    public CameraMovement cameraMovement;
    public GameObject placementManagerGameObject;
    public UIController uiController;
    public StructureRepository structureRepository;
    public GameObject resourceManagerGameObject;
    private IResourceManager resourceManager;

    public PlayerSelectionState playerSelectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;
    public PlayerBuildingRoadState buildingRoadState;
    public PlayerDemolishingState playerRemoveBuildingState;

    private IPlacementManager placementManager;


    public PlayerState PlayerState 
    { 
        get => playerState;
    }

    private void Awake()
    {
        #if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
                inputManager = gameObject.AddComponent<InputManager>();
        #endif
    }

    private void PrepareStates()
    {
        buildingManager = new BuildingManager(placementManager, structureRepository, resourceManager, cellSize, width, lenght);
        resourceManager.PrepareResourceManager(buildingManager);
        playerSelectionState = new PlayerSelectionState(this, buildingManager);
        playerRemoveBuildingState = new PlayerDemolishingState(this, buildingManager);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, buildingManager);
        buildingRoadState = new PlayerBuildingRoadState(this, buildingManager);
        playerState = playerSelectionState;
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
