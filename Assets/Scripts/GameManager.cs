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
    public PlacementManager placementManager;
    public UIController uiController;

    void Start()
    {
        inputManager = FindObjectsOfType<MonoBehaviour>().OfType<IInputManager>().FirstOrDefault();
        grid = new GridStructure(cellSize, width, length);
        inputManager.AddListenerOnPointerDownEvent(HandleInput);
        uiController.AddListenerOnBuildAreaEvent(EnableBuildingMode);
        uiController.AddListenerOnCancelActionEvent(DisableBuildingMode);
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
