using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    GridStructure gridStructure;
    PlacementManager placementManager;
    ResourceManager resourceManager;

    public BuildingManager(PlacementManager placementManager, ResourceManager resourceManager,
            int cellSize, int width, int length)
    {
        this.gridStructure = new GridStructure(cellSize, width, length);
        this.placementManager = placementManager;
        this.resourceManager = resourceManager;
    }

    public void PlaceStructureAt(Vector3 position)
    {
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        if (!gridStructure.IsCellTaken(gridPosition))
        {
            placementManager.CreateBuilding(gridPosition, gridStructure, resourceManager);
        }
    }

    public void RemoveBuildingAt(Vector3 position)
    {
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        if (gridStructure.IsCellTaken(gridPosition))
        {
            placementManager.RemoveBuilding(gridPosition, gridStructure);
        }
    }
}
