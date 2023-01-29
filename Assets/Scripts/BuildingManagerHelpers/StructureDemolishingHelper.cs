using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureDemolishingHelper : StructureModificationHelper
{
    public StructureDemolishingHelper(StructureRepository structureRepository, GridStructure gridStructure, IPlacementManager placementManager,
        ResourceManager resourceManager) : base(structureRepository, gridStructure, placementManager, resourceManager) { }
    public override void CancelModifications()
    {
        this.placementManager.DisplayStructureOnMap(structuresToBeModified.Values);
        structuresToBeModified.Clear();
    }

    public override void ConfirmModifications()
    {
        foreach (Vector3 pos in structuresToBeModified.Keys)
        {
            gridStructure.RemoveStructureFromTheGrid(pos);
        }
        this.placementManager.DestroyDisplayedStructures(structuresToBeModified.Values);
        structuresToBeModified.Clear();
    }

    public override void PrepareStructureForModification(Vector3 position, string structureName, StructureType structureType)
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
                placementManager.SetStructureForDemolishing(structure);
                //placementManager.RemoveBuilding(gridPosition, gridStructure);
            }

        }
    }
}
