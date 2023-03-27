using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureDemolishingHelper : StructureModificationHelper
{
    Dictionary<Vector3Int, GameObject> roadsToDemolish = new Dictionary<Vector3Int, GameObject>();

    public StructureDemolishingHelper(StructureRepository structureRepository, GridStructure gridStructure) 
        : base(structureRepository, gridStructure) { }
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
        var roadPlacementHelper = StructureModificationFactory.GetHelper(typeof(PlayerBuildingRoadState));
        foreach (KeyValuePair<Vector3Int, GameObject> kvp in roadsToDemolish)
        {
            Dictionary<Vector3Int, GameObject> neighbours = RoadManager.GetRoadNeighboursForPosition(gridStructure, kvp.Key);
            if(neighbours.Count > 0)
            {
                var structureBase = gridStructure.GetStructureBaseFromGrid(neighbours.Keys.First());
                RoadManager.modifyRoadCellsOnGrid(neighbours, structureBase, new Dictionary<Vector3Int, GameObject>(), gridStructure);
            }
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
                if (RoadManager.CheckIfNeighbourIsRoadOnGrid(gridStructure, gridPositionInt)
                    && roadsToDemolish.ContainsKey(gridPositionInt))
                {
                    roadsToDemolish.Remove(gridPositionInt);
                }
            }
            else
            {
                // Add structure to demolish list
                structuresToBeModified.Add(gridPositionInt, structure);
                placementManager.SetStructureForDemolishing(structure);
                if (RoadManager.CheckIfNeighbourIsRoadOnGrid(gridStructure, gridPositionInt)
                    && !roadsToDemolish.ContainsKey(gridPositionInt))
                {
                    roadsToDemolish.Add(gridPositionInt, structure);
                }
                //placementManager.RemoveBuilding(gridPosition, gridStructure);
            }

        }
    }
}
