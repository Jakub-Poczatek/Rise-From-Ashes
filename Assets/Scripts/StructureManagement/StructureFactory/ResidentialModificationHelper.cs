using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentialModificationHelper : StructureModificationHelper
{
    public ResidentialModificationHelper(StructureRepository structureRepository, GridStructure gridStructure)
        : base(structureRepository, gridStructure) { }

    public override void PrepareStructureForModification(Vector3 position, string structureName = "", StructureType structureType = StructureType.None)
    {
        base.PrepareStructureForModification(position, structureName, structureType);

        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            RemovePreview(gridPosition, gridPositionInt);
        }
        else if (!gridStructure.IsCellTaken(gridPosition) && resourceManager.CanIAffordIt(structureBase.buildCost))
        {
            AddPreview(structureBase, gridPosition, gridPositionInt);
        }
    }

    private void AddPreview(StructureBase myStructureBase, Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        // Add preview structure
        // ghostReturn = (structure, position, gridOutline)
        (GameObject, Vector3, GameObject)? ghostReturn = placementManager.CreateGhostResidential(gridPosition, myStructureBase, gridStructure);
        if (ghostReturn != null)
        {
            gridPositionInt = Vector3Int.FloorToInt(ghostReturn.Value.Item2);
            structuresToBeModified.Add(gridPositionInt, ghostReturn.Value.Item1);
            gridStructure.PlaceStructureOnTheGrid(ghostReturn.Value.Item1, ghostReturn.Value.Item2, myStructureBase, ghostReturn.Value.Item3);
            resourceManager.Purchase(structureBase.buildCost);
            resourceManager.MaxCitizenCapacity++;
        }
    }

    private void RemovePreview(Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        // Remove preview structure
        GameObject structure = structuresToBeModified[gridPositionInt];
        MonoBehaviour.Destroy(structure);
        gridStructure.RemoveStructureFromTheGrid(gridPosition);
        structuresToBeModified.Remove(gridPositionInt);
        resourceManager.EarnResources(structureBase.buildCost);
        resourceManager.MaxCitizenCapacity--;
    }

    public override void CancelModifications()
    {
        /*foreach (var structure in structuresToBeModified)
        {
            resourceManager.EarnResources(structureBase.buildCost);
            resourceManager.EvictCitizen();
        }*/
        int structAmount = structuresToBeModified.Count;
        resourceManager.EarnResources(new Cost(
            structAmount * structureBase.buildCost.gold,
            structAmount * structureBase.buildCost.food,
            structAmount * structureBase.buildCost.wood,
            structAmount * structureBase.buildCost.stone,
            structAmount * structureBase.buildCost.metal
            ));
        for (int i = 0; i < structAmount; i++)
            resourceManager.MaxCitizenCapacity--;
        base.CancelModifications();
    }
}
