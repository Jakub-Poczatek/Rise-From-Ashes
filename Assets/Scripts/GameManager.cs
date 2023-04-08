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
    private bool buildingModeIsActive = false;
    private GridStructure gridStructure;
    private PlayerState playerState;
    private int width, length;
    //public IInputManager inputManager;
    //public InputManager inputManagerConcrete;
    public InputManager inputManager;
    public LayerMask mouseInputMask;
    public CameraMovement cameraMovement;
    public GameObject placementManagerGameObject;
    public UIController uiController;
    public StructureRepository structureRepository;
    //public GameObject resourceManagerGameObject;
    //private IResourceManager resourceManager;
    private ResourceManager resourceManager;

    public PlayerSelectionState selectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;
    public PlayerBuildingRoadState buildingRoadState;
    public PlayerBuildingResidentialState buildingResidentialState;
    public PlayerDemolishingState removeBuildingState;
    public PlayerCitizenAssignState citizenAssignState;

    //private IPlacementManager placementManager;
    private PlacementManager placementManager;


    public PlayerState PlayerState { get => playerState; }

    private void Awake()
    {
        /*#if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
        inputManager = gameObject.AddComponent<InputManager>();
        inputManager = inputManagerConcrete;
        #endif*/
        //inputManager = inputManagerConcrete;
        //inputManager = gameObject.AddComponent<InputManager>();
        resourceManager = ResourceManager.Instance;
        placementManager = PlacementManager.Instance;
    }

    void Start()
    {
        width = (int)groundModel.GetComponent<MeshRenderer>().bounds.size.x;
        length = (int)groundModel.GetComponent<MeshRenderer>().bounds.size.z;
        //placementManager = placementManagerGameObject.GetComponent<IPlacementManager>();
        //resourceManager = resourceManagerGameObject.GetComponent<IResourceManager>();
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
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this);
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
        inputManager.AddListenerOnCameraRotatePerformedEvent((angle) => PlayerState.OnCameraRotate(angle));
        inputManager.AddListenerOnCameraZoomPerformedEvent((zoom) => PlayerState.OnCameraZoom(zoom));
        inputManager.AddListenerOnCameraMoveChangeEvent((direction) => playerState.OnCameraMove(direction));
    }

    public void TransitionToState(PlayerState newState, string structureName)
    {
        this.playerState = newState;
        this.playerState.EnterState(structureName);
    }
}
