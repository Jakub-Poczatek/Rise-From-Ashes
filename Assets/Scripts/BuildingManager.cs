using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    GridStructure gridStructure;
    IPlacementManager placementManager;
    ResourceManager resourceManager;
    StructureRepository structureRepository;
    StructureModificationHelper helper;

    public BuildingManager(IPlacementManager placementManager, ResourceManager resourceManager,
                        StructureRepository structureRepository, int cellSize, int width, int length)
    {
        this.gridStructure = new GridStructure(cellSize, width, length);
        this.placementManager = placementManager;
        this.resourceManager = resourceManager;
        this.structureRepository = structureRepository;
        StructureModificationFactory.PrepareStructureModificationFactory(structureRepository, gridStructure, placementManager, resourceManager);
    }

    public void PrepareBuildingManager(Type classType)
    {
        helper = StructureModificationFactory.GetHelper(classType);
    }

    public void PrepareStructureForModification(Vector3 position, string structureName, StructureType structureType)
    {
        helper.PrepareStructureForModification(position, structureName, structureType);
    }

    public void ConfirmModification()
    {
        helper.ConfirmModifications();
    }

    public void CancelModification()
    {
        helper.CancelModifications();
    }

    public void PrepareStructureForDemolishing(Vector3 position)
    {
        helper.PrepareStructureForModification(position);
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
        GameObject structureToReturn = helper.GetStructureToBeModified(gridPosition);
        if(structureToReturn != null)
        {
            return structureToReturn;
        }
        structureToReturn = helper.GetStructureToBeModified(gridPosition);
        return structureToReturn;
    }
}
