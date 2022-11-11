using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int cellSize = 1;
    private GridStructure grid;
    public int width, length;
    public PlacementManager placementManager;
    public InputManager inputManager;

    void Start()
    {
        grid = new GridStructure(cellSize, width, length);
        inputManager.AddListenerOnPointerDownEvent(HandleInput);
    }

    private void HandleInput(Vector3 position)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(position);
        if (grid.IsCellTaken(gridPosition)==false)
        {
            placementManager.CreateBuilding(gridPosition, grid);
        }
    }
}
