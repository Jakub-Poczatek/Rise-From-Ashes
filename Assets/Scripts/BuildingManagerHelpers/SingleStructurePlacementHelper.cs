using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleStructurePlacementHelper : StructureModificationHelper
{
    StructureBase structureBase;
    public SingleStructurePlacementHelper(StructureRepository structureRepository, GridStructure gridStructure, IPlacementManager placementManager, IResourceManager resourceManager) 
        : base(structureRepository, gridStructure, placementManager, resourceManager) { }

    public override void PrepareStructureForModification(Vector3 position, string structureName, StructureType structureType)
    {
        structureBase = this.structureRepository.GetStructureByName(structureName, structureType);
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);

        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            resourceManager.InceaseGold(structureBase.buildCost.gold);
            RemovePreview(gridPosition, gridPositionInt);
        }
        else if (!gridStructure.IsCellTaken(gridPosition) && resourceManager.CanIBuyIt(structureBase.buildCost.gold))
        {
            AddPreview(structureBase, gridPosition, gridPositionInt);
        }
    }

    private void AddPreview(StructureBase myStructureBase, Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        // Add preview structure
        // ghostReturn = (structure, position, gridOutline)
        (GameObject, Vector3, GameObject)? ghostReturn = placementManager.CreateGhostStructure(gridPosition, myStructureBase, gridStructure);
        if (ghostReturn != null)
        {
            gridPositionInt = Vector3Int.FloorToInt(ghostReturn.Value.Item2);
            structuresToBeModified.Add(gridPositionInt, ghostReturn.Value.Item1);
            gridStructure.PlaceStructureOnTheGrid(ghostReturn.Value.Item1, ghostReturn.Value.Item2, myStructureBase, ghostReturn.Value.Item3);
            resourceManager.SpendGold(structureBase.buildCost.gold);
        }
    }

    private void RemovePreview(Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        // Remove preview structure
        GameObject structure = structuresToBeModified[gridPositionInt];
        MonoBehaviour.Destroy(structure);
        gridStructure.RemoveStructureFromTheGrid(gridPosition);
        structuresToBeModified.Remove(gridPositionInt);
    }

    public override void CancelModifications()
    {
        foreach (var structure in structuresToBeModified)
        {
            resourceManager.InceaseGold(structureBase.buildCost.gold);
        }
        base.CancelModifications();
    }
}
