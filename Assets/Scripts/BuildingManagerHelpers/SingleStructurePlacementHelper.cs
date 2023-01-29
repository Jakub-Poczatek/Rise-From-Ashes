using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleStructurePlacementHelper : StructureModificationHelper
{
    public SingleStructurePlacementHelper(StructureRepository structureRepository, GridStructure gridStructure, PlacementManager placementManager, 
        ResourceManager resourceManager) : base(structureRepository, gridStructure, placementManager, resourceManager) { }

    public override void PrepareStructureForModification(Vector3 position, string structureName, StructureType structureType)
    {
        Time.timeScale = 0;
        StructureBase structureBase = this.structureRepository.GetStructureByName(structureName, structureType);
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);

        if (!resourceManager.isAffordable(structureBase))
        {
            return;
        }

        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            // Remove preview structure
            GameObject structure = structuresToBeModified[gridPositionInt];
            MonoBehaviour.Destroy(structure);
            gridStructure.RemoveStructureFromTheGrid(gridPosition);
            structuresToBeModified.Remove(gridPositionInt);
        }
        else if (!gridStructure.IsCellTaken(gridPosition))
        {
            // Add preview structure
            // ghostReturn = (structure, position, gridOutline)
            (GameObject, Vector3, GameObject)? ghostReturn = placementManager.CreateGhostStructure(gridPosition, structureBase, gridStructure, resourceManager);
            if (ghostReturn != null)
            {
                gridPositionInt = Vector3Int.FloorToInt(ghostReturn.Value.Item2);
                structuresToBeModified.Add(gridPositionInt, ghostReturn.Value.Item1);
                gridStructure.PlaceStructureOnTheGrid(ghostReturn.Value.Item1, ghostReturn.Value.Item2, ghostReturn.Value.Item3);
            }
        }
    }

    public override void ConfirmModifications()
    {
        placementManager.DisplayStructureOnMap(structuresToBeModified.Values);
        structuresToBeModified.Clear();
        resourceManager.SyncResourceGains();
        Time.timeScale = 1;
    }

    public override void CancelModifications()
    {
        placementManager.DestroyDisplayedStructures(structuresToBeModified.Values);
        foreach (var keyValuePair in structuresToBeModified)
        {
            gridStructure.RemoveStructureFromTheGrid(keyValuePair.Key);
        }
        structuresToBeModified.Clear();
        resourceManager.ClearTempResourceGain();
        Time.timeScale = 1;
    }

}
