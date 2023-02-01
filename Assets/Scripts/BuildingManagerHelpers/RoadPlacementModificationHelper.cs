using NSubstitute.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoadPlacementModificationHelper : StructureModificationHelper
{
    StructureBase structureBase;
    Dictionary<Vector3Int, GameObject> existingRoadStructuresToBeModified = new Dictionary<Vector3Int, GameObject>();

    public RoadPlacementModificationHelper(StructureRepository structureRepository, GridStructure gridStructure, IPlacementManager placementManager, 
        ResourceManager resourceManager) : base(structureRepository, gridStructure, placementManager, resourceManager)
    {

    }

    public override void PrepareStructureForModification(Vector3 position, string structureName = "", StructureType structureType = StructureType.None)
    {
        structureBase = structureRepository.GetStructureByName(structureName, structureType);
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);

        if (!structuresToBeModified.ContainsKey(gridPositionInt) && gridStructure.IsCellTaken(gridPosition)) return;

        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            RevokeRoadPlacement(gridPosition, gridPositionInt);
        }
        else if (!gridStructure.IsCellTaken(gridPosition))
        {
            RoadStructureHelper road = GetCorrectRoadPrefab(gridPosition, structureBase);
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
                RoadStructureHelper neighboursStructure = GetCorrectRoadPrefab(neighbourPositionInt, structureBase);
                PlaceNewRoad(neighboursStructure, neighbourPositionInt, neighbourPositionInt);
                //StructureBase neighbourStructureData = gridStructure.GetStructureDataFromGrid(neighbourGridPosition.Value);
                //if (neighbourStructureData != null && neighbourStructureData.GetType() == typeof(RoadStruct) && !existingRoadStructuresToBeModified.ContainsKey(neighbourPositionInt))
                //{
                //    existingRoadStructuresToBeModified.Add(neighbourPositionInt, gridStructure.GetStructureFromTheGrid(neighbourGridPosition.Value));
                //} 
            }
        }
    }

    private RoadStructureHelper GetCorrectRoadPrefab(Vector3 position, StructureBase structureBase)
    {
        int neighboursStatus = RoadManager.GetRoadNeighboursStatus(position, gridStructure, structuresToBeModified);
        RoadStructureHelper roadToReturn = null;
        roadToReturn = RoadManager.IfStraightRoadFits(neighboursStatus, roadToReturn, structureBase);
        if (roadToReturn != null)
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

    public override void CancelModifications()
    {
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

    public void modifyRoadCellsOnGrid(Dictionary<Vector3Int, GameObject> neighbours, StructureBase structureBase)
    {
        foreach (KeyValuePair<Vector3Int, GameObject> kvp in neighbours)
        {
            if (structuresToBeModified.ContainsKey(kvp.Key)) structuresToBeModified.Remove(kvp.Key);
            MonoBehaviour.Destroy(kvp.Value);
            gridStructure.RemoveStructureFromTheGrid(kvp.Key);
            RoadStructureHelper roadStruct = GetCorrectRoadPrefab(kvp.Key, structureBase);
            PlaceNewRoad(roadStruct, kvp.Key, kvp.Key);
            //var structure =
                //placementManager.InstantiateRoad(kvp.Key, roadStruct.RoadPrefab, roadStruct.RoadPrefabRotation);
                //placementManager.CreateGhostRoad(kvp.Key, roadStruct.RoadPrefab, gridStructure, roadStruct.RoadPrefabRotation);
            //gridStructure.PlaceStructureOnTheGrid(structure.Value.Item1, kvp.Key, structureBase);
            //structuresToBeModified.Add(kvp.Key, gridStructure.GetStructureFromTheGrid(kvp.Key));
        }
        placementManager.DisplayStructureOnMap(structuresToBeModified.Values);
        neighbours.Clear();
    }
}
