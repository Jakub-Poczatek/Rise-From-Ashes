using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlacementModificationHelper : StructureModificationHelper
{
    StructureBase structureBase;

    public RoadPlacementModificationHelper(StructureRepository structureRepository, GridStructure gridStructure, IPlacementManager placementManager, 
        ResourceManager resourceManager) : base(structureRepository, gridStructure, placementManager, resourceManager)
    {

    }

    public override void PrepareStructureForModification(Vector3 position, string structureName = "", StructureType structureType = StructureType.None)
    {
        structureBase = structureRepository.GetStructureByName(structureName, structureType);
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);

        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            RevokeRoadPlacement(gridPosition, gridPositionInt);
        }
        else if (!gridStructure.IsCellTaken(gridPosition))
        {
            RoadStructureHelper road = GetCorrectRoadPrefab(gridPosition);
            gridPositionInt = PlaceNewRoad(road, gridPosition, gridPositionInt);
        }
        AdjustNeighboursIfAreRoadStructures(gridPosition);
    }

    private Vector3Int PlaceNewRoad(RoadStructureHelper road, Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        (GameObject, Vector3, GameObject)? ghostReturn = placementManager.CreateGhostRoad(gridPosition, road.RoadPrefab, gridStructure, road.RoadPrefabRotation);
        if (ghostReturn != null)
        {
            //Debug.Log("Ghost return is not null");
            gridPositionInt = Vector3Int.FloorToInt(ghostReturn.Value.Item2);
            structuresToBeModified.Add(gridPositionInt, ghostReturn.Value.Item1);
            gridStructure.PlaceStructureOnTheGrid(ghostReturn.Value.Item1, ghostReturn.Value.Item2, structureBase, ghostReturn.Value.Item3);
        }

        return gridPositionInt;
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
            if (RoadManager.CheckIfNeighbourIsRoadInDictionary(neighbourPositionInt, structuresToBeModified))
            {
                RevokeRoadPlacement(gridPosition, neighbourPositionInt);
                RoadStructureHelper neighboursStructure = GetCorrectRoadPrefab(neighbourGridPosition.Value);
                PlaceNewRoad(neighboursStructure, neighbourGridPosition.Value, neighbourPositionInt);
            } else if(RoadManager.CheckIfNeighbourIsRoadOnGrid(gridStructure, neighbourPositionInt))
            {

            }
        }
    }

    private RoadStructureHelper GetCorrectRoadPrefab(Vector3 position)
    {
        int neighboursStatus = RoadManager.GetRoadNeighboursStatus(position, gridStructure, structuresToBeModified);
        RoadStructureHelper roadToReturn = null;
        roadToReturn = RoadManager.IfStraightRoadFits(neighboursStatus, roadToReturn, structureBase);
        if(roadToReturn != null)
        {
            return roadToReturn;
        }
        roadToReturn = RoadManager.IfCornerRoadFits(neighboursStatus, roadToReturn, structureBase);
        if (roadToReturn != null)
        {
            return roadToReturn;
        }
        roadToReturn = RoadManager.IfThreeWayFits(neighboursStatus, roadToReturn, structureBase);
        if (roadToReturn != null)
        {
            return roadToReturn;
        }
        roadToReturn = RoadManager.IfFourWayFits(neighboursStatus, roadToReturn, structureBase);
        return roadToReturn;
    }

    
}
