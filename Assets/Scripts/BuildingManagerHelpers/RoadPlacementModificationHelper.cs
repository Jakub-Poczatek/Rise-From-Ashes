using NSubstitute.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoadPlacementModificationHelper : StructureModificationHelper
{
    Dictionary<Vector3Int, GameObject> existingRoadStructuresToBeModified = new Dictionary<Vector3Int, GameObject>();

    public RoadPlacementModificationHelper(StructureRepository structureRepository, GridStructure gridStructure, 
        IPlacementManager placementManager, IResourceManager resourceManager) : base(structureRepository, gridStructure, placementManager, resourceManager)
    {

    }

    public override void PrepareStructureForModification(Vector3 position, string structureName = "", StructureType structureType = StructureType.None)
    {
        base.PrepareStructureForModification(position, structureName, structureType);

        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);

        if (!structuresToBeModified.ContainsKey(gridPositionInt) && gridStructure.IsCellTaken(gridPosition)) return;

        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            RevokeRoadPlacement(gridPosition, gridPositionInt);
            resourceManager.InceaseGold(structureBase.buildCost.gold);
        }
        else if (!gridStructure.IsCellTaken(gridPosition) && resourceManager.CanIBuyIt(structureBase.buildCost.gold))
        {
            RoadStructureHelper road = RoadManager.GetCorrectRoadPrefab(gridPosition, structureBase, structuresToBeModified, gridStructure);
            gridPositionInt = RoadManager.PlaceNewRoad(road, gridPosition, gridPositionInt, placementManager, gridStructure, structuresToBeModified, structureBase);
            resourceManager.SpendGold(structureBase.buildCost.gold);
        }
        AdjustNeighboursIfAreRoadStructures(gridPosition);
    }

    

    private void RevokeRoadPlacement(Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        GameObject structure = structuresToBeModified[gridPositionInt];
        MonoBehaviour.Destroy(structure);
        gridStructure.RemoveStructureFromTheGrid(gridPosition);
        structuresToBeModified.Remove(gridPositionInt);
    }

    private void AdjustNeighboursIfAreRoadStructures(Vector3 gridPosition)
    {
        AdjustNeighboursIfRoad(gridPosition, NeighbourDirection.Up);
        AdjustNeighboursIfRoad(gridPosition, NeighbourDirection.Down);
        AdjustNeighboursIfRoad(gridPosition, NeighbourDirection.Right);
        AdjustNeighboursIfRoad(gridPosition, NeighbourDirection.Left);
    }

    private void AdjustNeighboursIfRoad(Vector3 gridPosition, NeighbourDirection direction)
    {
        Vector3Int? neighbourGridPosition = gridStructure.GetNeighbourPositionNullable(gridPosition, direction);
        if (neighbourGridPosition.HasValue)
        {
            Vector3Int neighbourPositionInt = neighbourGridPosition.Value;
            /*if (RoadManager.CheckIfNeighbourIsRoadInDictionary(neighbourPositionInt, structuresToBeModified))
            {
                Debug.Log("This is the value for neighbour in dictionary: " + direction.ToString() + " " + neighbourGridPosition.ToString());
                RevokeRoadPlacement(gridPosition, neighbourPositionInt);
                RoadStructureHelper neighboursStructure = GetCorrectRoadPrefab(neighbourPositionInt);
                PlaceNewRoad(neighboursStructure, neighbourPositionInt, neighbourPositionInt);
            }*/
            if(RoadManager.CheckIfNeighbourIsRoadOnGrid(gridStructure, neighbourPositionInt))
            {
                if (structuresToBeModified.ContainsKey(neighbourPositionInt)) structuresToBeModified.Remove(neighbourPositionInt);
                GameObject structure = gridStructure.GetStructureFromTheGrid(neighbourPositionInt);
                MonoBehaviour.Destroy(structure);
                gridStructure.RemoveStructureFromTheGrid(neighbourPositionInt);
                //structuresToBeModified.Remove(gridPositionInt);
                RoadStructureHelper neighboursStructure = RoadManager.GetCorrectRoadPrefab(neighbourPositionInt, structureBase, structuresToBeModified, gridStructure);
                RoadManager.PlaceNewRoad(neighboursStructure, neighbourPositionInt, neighbourPositionInt, placementManager, gridStructure, structuresToBeModified, structureBase);
                //StructureBase neighbourStructureData = gridStructure.GetStructureDataFromGrid(neighbourGridPosition.Value);
                //if (neighbourStructureData != null && neighbourStructureData.GetType() == typeof(RoadStruct) && !existingRoadStructuresToBeModified.ContainsKey(neighbourPositionInt))
                //{
                //    existingRoadStructuresToBeModified.Add(neighbourPositionInt, gridStructure.GetStructureFromTheGrid(neighbourGridPosition.Value));
                //} 
            }
        }
    }

    public override void CancelModifications()
    {
        resourceManager.InceaseGold(structuresToBeModified.Count * structureBase.buildCost.gold);
        base.CancelModifications();
        existingRoadStructuresToBeModified.Clear();
    }

    public override void ConfirmModifications()
    {
        /*foreach (KeyValuePair<Vector3Int, GameObject> kvp in existingRoadStructuresToBeModified)
        {
            gridStructure.RemoveStructureFromTheGrid(kvp.Key);
            MonoBehaviour.Destroy(kvp.Value);
            RoadStructureHelper roadStructure = GetCorrectRoadPrefab(kvp.Key);

            var structure =
                placementManager.CreateGhostRoad(kvp.Key, roadStructure.RoadPrefab, gridStructure, roadStructure.RoadPrefabRotation);
            gridStructure.PlaceStructureOnTheGrid(structure.Value.Item1, kvp.Key, structureBase);
            structuresToBeModified.Add(kvp.Key, gridStructure.GetStructureFromTheGrid(kvp.Key));
        }*/
        placementManager.DisplayStructureOnMap(structuresToBeModified.Values);
        existingRoadStructuresToBeModified.Clear();
        base.ConfirmModifications();
    }

    
}
