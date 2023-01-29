using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureDemolishingHelper
{
    Dictionary<Vector3Int, GameObject> structuresToBeModified = new Dictionary<Vector3Int, GameObject>();
    private StructureRepository structureRepository;
    private GridStructure gridStructure;
    private PlacementManager placementManager;
    private ResourceManager resourceManager;

    public StructureDemolishingHelper(StructureRepository structureRepository, GridStructure gridStructure,
        PlacementManager placementManager, ResourceManager resourceManager)
    {
        this.structureRepository = structureRepository;
        this.gridStructure = gridStructure;
        this.placementManager = placementManager;
        this.resourceManager = resourceManager;
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
            }
            else
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

    public GameObject GetStructureToBeModified(Vector3 gridPosition)
    {
        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            return structuresToBeModified[gridPositionInt];
        }
        return null;
    }
}
