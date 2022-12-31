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

        if (!gridStructure.IsCellTaken(gridPosition))
        {
            placementManager.CreateBuilding(gridPosition, gridStructure, structureBase, resourceManager);
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
