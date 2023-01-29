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

    public void PrepareStructureForPlacement(Vector3 position, string structureName, StructureType structureType)
    {
        Time.timeScale = 0;
        StructureBase structureBase = this.structureRepository.GetStructureByName(structureName, structureType);
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);

        if (!resourceManager.isAffordable(structureBase))
        {
            return;
        }

        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (!gridStructure.IsCellTaken(gridPosition))
        {
            if (structuresToBeModified.ContainsKey(gridPositionInt))
            {
                // Remove preview structure
                GameObject structure = structuresToBeModified[gridPositionInt];
                MonoBehaviour.Destroy(structure);
                structuresToBeModified.Remove(gridPositionInt);
            }
            else
            {
                // Add preview structure
                (GameObject, Vector3, GameObject)? ghostReturn = placementManager.CreateGhostStructure(gridPosition, structureBase, gridStructure, resourceManager);
                if (ghostReturn != null)
                {
                    structuresToBeModified.Add(gridPositionInt, ghostReturn.Value.Item1);
                    gridStructure.PlaceStructureOnTheGrid(ghostReturn.Value.Item1, ghostReturn.Value.Item2, ghostReturn.Value.Item3);
                }
            }
        }
    }

    public void ConfirmPlacement()
    {
        placementManager.DisplayStructureOnMap(structuresToBeModified.Values);
        structuresToBeModified.Clear();
        resourceManager.SyncResourceGains();
        Time.timeScale = 1;
    }

    public void CancelPlacement()
    {
        placementManager.DestroyDisplayedStructures(structuresToBeModified.Values);
        foreach (var keyValuePair in structuresToBeModified)
        {
            //gridStructure.PlaceStructureOnTheGrid(keyValuePair.Value, keyValuePair.Key);
            gridStructure.RemoveStructureFromTheGrid(keyValuePair.Key);
        }
        structuresToBeModified.Clear();
        resourceManager.ClearTempResourceGain();
        Time.timeScale = 1;
    }

    public void PrepareStructureForDemolishing(Vector3 position)
    {
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        if (gridStructure.IsCellTaken(gridPosition))
        {
            Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            GameObject structure = gridStructure.GetStructureFromTheGrid(gridPosition);
            if (structuresToBeModified.ContainsKey(gridPositionInt))
            {
                // Cancel structure demolish
                placementManager.ResetBuildingMaterial(structure);
                structuresToBeModified.Remove(gridPositionInt);
            } else
            {
                // Add structure to demolish list
                structuresToBeModified.Add(gridPositionInt, structure);
                placementManager.SetBuildingForDemolishing(structure);
                //placementManager.RemoveBuilding(gridPosition, gridStructure);
            }

        }
    }
    
    public void CancelDemolishing()
    {
        this.placementManager.DisplayStructureOnMap(structuresToBeModified.Values);
        structuresToBeModified.Clear();
    }

    public void ConfirmDemolishing()
    {
        foreach (Vector3 pos in structuresToBeModified.Keys)
        {
            gridStructure.RemoveStructureFromTheGrid(pos);
        }
        this.placementManager.DestroyDisplayedStructures(structuresToBeModified.Values);
        structuresToBeModified.Clear();
    }
}
