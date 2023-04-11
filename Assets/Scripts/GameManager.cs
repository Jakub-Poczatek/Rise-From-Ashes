using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int cellSize = 1;
    public GameObject groundModel;
    private PlayerState playerState;
    private int width, length;
    public InputManager inputManager;
    public LayerMask mouseInputMask;
    public CameraMovement cameraMovement;
    public GameObject placementManagerGameObject;
    public UIController uiController;
    public StructureRepository structureRepository;
    private ResourceManager resourceManager;

    public PlayerSelectionState selectionState;
    public PlayerBuildingResGenStructureState buildingResGenStructureState;
    public PlayerBuildingRoadState buildingRoadState;
    public PlayerBuildingResidentialState buildingResidentialState;
    public PlayerDemolishingState removeBuildingState;
    public PlayerCitizenAssignState citizenAssignState;

    private PlacementManager placementManager;

    public PlayerState PlayerState { get => playerState; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        resourceManager = ResourceManager.Instance;
        placementManager = PlacementManager.Instance;
    }

    public static GameManager Instance { get; private set; }

    private GameManager() {}

    void Start()
    {
        width = (int)groundModel.GetComponent<MeshRenderer>().bounds.size.x;
        length = (int)groundModel.GetComponent<MeshRenderer>().bounds.size.z;
        PrepareStates();
        PrepareGameComponents();
        AssignInputListener();
        AssignUiControllerListeners();
    }

    private void PrepareStates()
    {
        BuildingManager.Instance.PrepareBuildingManager(structureRepository, cellSize, width, length);
        resourceManager.PrepareResourceManager();
        selectionState = new PlayerSelectionState(this);
        removeBuildingState = new PlayerDemolishingState(this);
        buildingResGenStructureState = new PlayerBuildingResGenStructureState(this);
        buildingRoadState = new PlayerBuildingRoadState(this);
        buildingResidentialState = new PlayerBuildingResidentialState(this);
        citizenAssignState = new PlayerCitizenAssignState(this);
        playerState = selectionState;
    }


    private void Update()
    {
        uiController.UpdateDebugDisplay(playerState.ToString());
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
        uiController.AddListenerOnBuildResidentialEvent((structureName) => playerState.OnBuildResidential(structureName));
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
        inputManager.AddListenerOnCameraRotatePerformedEvent((angle) => PlayerState.OnRotate(angle));
        inputManager.AddListenerOnCameraZoomPerformedEvent((zoom) => PlayerState.OnCameraZoom(zoom));
        inputManager.AddListenerOnCameraMoveChangeEvent((direction) => playerState.OnCameraMove(direction));
    }

    public void TransitionToState(PlayerState newState, string structureName)
    {
        this.playerState = newState;
        this.playerState.EnterState(structureName);
    }
}
