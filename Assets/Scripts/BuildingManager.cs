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
    SingleStructurePlacementHelper singleStructurePlacementHelper;
    StructureDemolishingHelper structureDemolishingHelper;

    public BuildingManager(PlacementManager placementManager, ResourceManager resourceManager,
                        StructureRepository structureRepository, int cellSize, int width, int length)
    {
        this.gridStructure = new GridStructure(cellSize, width, length);
        this.placementManager = placementManager;
        this.resourceManager = resourceManager;
        this.structureRepository = structureRepository;
        singleStructurePlacementHelper =
            new(structureRepository, gridStructure, placementManager, resourceManager);
        structureDemolishingHelper =
            new(structureRepository, gridStructure, placementManager, resourceManager);
    }

    public void PrepareStructureForPlacement(Vector3 position, string structureName, StructureType structureType)
    {
        singleStructurePlacementHelper.PrepareStructureForPlacement(position, structureName, structureType);
    }

    public void ConfirmPlacement()
    {
        singleStructurePlacementHelper.ConfirmPlacement();
    }

    public void CancelPlacement()
    {
        singleStructurePlacementHelper.CancelPlacement();
    }

    public void PrepareStructureForDemolishing(Vector3 position)
    {
        structureDemolishingHelper.PrepareStructureForDemolishing(position);
    }
    
    public void CancelDemolishing()
    {
        structureDemolishingHelper.CancelDemolishing();
    }

    public void ConfirmDemolishing()
    {
        structureDemolishingHelper.ConfirmDemolishing();
    }

    public GameObject GetStructureFromGrid(Vector3 position)
    {
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        if(gridStructure.IsCellTaken(gridPosition))
        {
            return gridStructure.GetStructureFromTheGrid(gridPosition);
        }
        return null;
    }

    public GameObject GetStructureToBeModified(Vector3 position)
    {
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        GameObject structureToReturn = singleStructurePlacementHelper.GetStructureToBeModified(gridPosition);
        if(structureToReturn != null)
        {
            return structureToReturn;
        }
        structureToReturn = structureDemolishingHelper.GetStructureToBeModified(gridPosition);
        return structureToReturn;
    }
}
