using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    GridStructure gridStructure;
    PlacementManager placementManager;
    ResourceManager resourceManager;
    StructureRepository structureRepository;
    Dictionary<Vector3Int, GameObject> structuresToBeModified = new Dictionary<Vector3Int, GameObject>(); 

    public BuildingManager(PlacementManager placementManager, ResourceManager resourceManager, 
                        StructureRepository structureRepository, int cellSize, int width, int length)
    {
        this.gridStructure = new GridStructure(cellSize, width, length);
        this.placementManager = placementManager;
        this.resourceManager = resourceManager;
        this.structureRepository = structureRepository;
    }

    public void PlaceStructureAt(Vector3 position, string structureName, StructureType structureType)
    {

        StructureBase structureBase = this.structureRepository.GetStructureByName(structureName, structureType);
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);

        if (!resourceManager.isAffordable(structureBase))
        {
            return;
        }

        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (!gridStructure.IsCellTaken(gridPosition) && !structuresToBeModified.ContainsKey(gridPositionInt))
        {
            //placementManager.CreateBuilding(gridPosition, gridStructure, structureBase, resourceManager);
            structuresToBeModified.Add(gridPositionInt, placementManager.CreateGhostStructure(gridPosition, structureBase));
        }
    }

    public void ConfirmPlacement()
    {
        placementManager.ConfirmPlacement(structuresToBeModified.Values);
        foreach (var keyValuePair in structuresToBeModified)
        {
            gridStructure.PlaceStructureOnTheGrid(keyValuePair.Value, keyValuePair.Key);
        }
        structuresToBeModified.Clear();
    }

    public void CancelPlacement()
    {
        placementManager.CancelPlacement(structuresToBeModified.Values);
        structuresToBeModified.Clear();
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
