using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int cellSize = 1;
    private bool buildingModeIsActive = false;
    private GridStructure grid;
    public int width, length;
    public IInputManager inputManager;
    public CameraMovement cameraMovement;
    public PlacementManager placementManager;
    public UIController uiController;

    void Start()
    {
        cameraMovement.SetCameraBounds(0, width, 0, length);
        inputManager = FindObjectsOfType<MonoBehaviour>().OfType<IInputManager>().FirstOrDefault();
        grid = new GridStructure(cellSize, width, length);
        inputManager.AddListenerOnPointerDownEvent(HandleInput);
        inputManager.AddListenerOnPointerSecondDownEvent(HandleInputCameraPan);
        inputManager.AddListenerOnPointerSecondUpEvent(HandleInptuCameraPanStop);
        uiController.AddListenerOnBuildAreaEvent(EnableBuildingMode);
        uiController.AddListenerOnCancelActionEvent(DisableBuildingMode);
    }

    private void HandleInptuCameraPanStop()
    {
        cameraMovement.StopCameraMovement();
    }

    private void HandleInputCameraPan(Vector3 position)
    {
        if (!buildingModeIsActive)
        {
            cameraMovement.MoveCamera(position);
        }
    }

    private void HandleInput(Vector3 position)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(position);
        if (buildingModeIsActive && grid.IsCellTaken(gridPosition)==false)
        {
            placementManager.CreateBuilding(gridPosition, grid);
        }
    }

    private void EnableBuildingMode()
    {
        buildingModeIsActive = true;
    }

    private void DisableBuildingMode()
    {
        buildingModeIsActive = false;
    }
}
