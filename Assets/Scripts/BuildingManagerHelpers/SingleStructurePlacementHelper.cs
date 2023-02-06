using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleStructurePlacementHelper : StructureModificationHelper
{
    public SingleStructurePlacementHelper(StructureRepository structureRepository, GridStructure gridStructure, IPlacementManager placementManager, IResourceManager resourceManager) 
        : base(structureRepository, gridStructure, placementManager, resourceManager) { }

    public override void PrepareStructureForModification(Vector3 position, string structureName, StructureType structureType)
    {
        Time.timeScale = 0;
        StructureBase myStructureBase = this.structureRepository.GetStructureByName(structureName, structureType);
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);

        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            resourceManager.InceaseGold(myStructureBase.buildCost.gold);
            RemovePreview(gridPosition, gridPositionInt);
        }
        else if (!gridStructure.IsCellTaken(gridPosition) && resourceManager.CanIBuyIt(myStructureBase.buildCost.gold))
        {
            AddPreview(myStructureBase, gridPosition, gridPositionInt);
            resourceManager.SpendGold(myStructureBase.buildCost.gold);
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


}
