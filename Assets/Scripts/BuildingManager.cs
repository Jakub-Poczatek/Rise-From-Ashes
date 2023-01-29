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
    StructureModificationHelper singleStructurePlacementHelper;
    StructureModificationHelper structureDemolishingHelper;

    public BuildingManager(PlacementManager placementManager, ResourceManager resourceManager,
                        StructureRepository structureRepository, int cellSize, int width, int length)
    {
        this.gridStructure = new GridStructure(cellSize, width, length);
        this.placementManager = placementManager;
        this.resourceManager = resourceManager;
        this.structureRepository = structureRepository;
        singleStructurePlacementHelper =
            new SingleStructurePlacementHelper(structureRepository, gridStructure, placementManager, resourceManager);
        structureDemolishingHelper =
            new StructureDemolishingHelper(structureRepository, gridStructure, placementManager, resourceManager);
    }

    public void PrepareStructureForPlacement(Vector3 position, string structureName, StructureType structureType)
    {
        singleStructurePlacementHelper.PrepareStructureForModification(position, structureName, structureType);
    }

    public void ConfirmPlacement()
    {
        singleStructurePlacementHelper.ConfirmModifications();
    }

    public void CancelPlacement()
    {
        singleStructurePlacementHelper.CancelModifications();
    }

    public void PrepareStructureForDemolishing(Vector3 position)
    {
        structureDemolishingHelper.PrepareStructureForModification(position);
    }
    
    public void CancelDemolishing()
    {
        structureDemolishingHelper.CancelModifications();
    }

    public void ConfirmDemolishing()
    {
        structureDemolishingHelper.ConfirmModifications();
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
